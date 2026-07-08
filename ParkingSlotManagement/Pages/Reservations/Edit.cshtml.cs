using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using ParkingSlotManagement.Models;

namespace ParkingSlotManagement.Pages.Reservations
{
    public class Edit : PageModel
    {
        [BindProperty]
        public ReservationInfo Reservation { get; set; } = new ReservationInfo();

        public string ErrorMessage{get; set;} = "";
        public string SuccessMessage{get; set;} = "";
        
        public void OnGet(int id)
        {
            try{
                string connectionString = "Server=localhost; Port=3306; Database=ParkingDB; Uid=root; Pwd=manager;";
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM Reservations WHERE Id=@Id", connection);
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Reservation.Id = reader.GetInt32(0);
                            Reservation.SlotId = reader.GetInt32(1);
                            Reservation.VehicleNo = reader.GetString(2);
                            Reservation.TimeIn = reader.GetDateTime(3);
                            Reservation.TimeOut = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = "Error reservation loading"+ex.Message;
            }
        }

        public void OnPost()
        {
            try{
                string connectionString = "Server=localhost; Port=3306; Database=ParkingDB; Uid=root; Pwd=manager;";
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("UPDATE Reservations SET SlotId=@SlotId, VehicleNo=@VehicleNo, TimeIn=@TimeIn, TimeOut=@TimeOut WHERE Id=@Id", connection);
                    command.Parameters.AddWithValue("@SlotId", Reservation.SlotId);
                    command.Parameters.AddWithValue("@VehicleNo", Reservation.VehicleNo);
                    command.Parameters.AddWithValue("@TimeIn", Reservation.TimeIn);
                    command.Parameters.AddWithValue("@TimeOut", Reservation.TimeOut.HasValue ? Reservation.TimeOut.Value : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Id", Reservation.Id);

                    command.ExecuteNonQuery();

                    // If TimeOut is set, free the slot
                    if (Reservation.TimeOut.HasValue)
                    {
                        var updateSlot = new MySqlCommand("UPDATE Slots SET Status='Available' WHERE Id=@Id", connection);
                        updateSlot.Parameters.AddWithValue("@Id", Reservation.SlotId);
                        updateSlot.ExecuteNonQuery();
                    }
                }
                SuccessMessage = "Reservation Edited Successfully";
                Response.Redirect("/Reservations/Index");
            }
            catch(Exception ex)
            {
                ErrorMessage = "Error reservation Editing"+ ex.Message;
            }
        }
    }
}