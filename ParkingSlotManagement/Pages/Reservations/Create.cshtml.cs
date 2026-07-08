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
    public class Create : PageModel
    {
        [BindProperty]
        public ReservationInfo Reservation { get; set; } = new ReservationInfo();

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public void OnGet() { }

        public void OnPost()
        {
            try
            {
                Reservation.TimeIn = DateTime.Now;

                string connectionString = "Server=localhost; Port=3306; Database=ParkingDB; Uid=root; Pwd=manager;";
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("INSERT INTO Reservations (SlotId, VehicleNo, TimeIn) VALUES (@SlotId, @VehicleNo, @TimeIn)", connection);
                    command.Parameters.AddWithValue("@SlotId", Reservation.SlotId);
                    command.Parameters.AddWithValue("@VehicleNo", Reservation.VehicleNo);
                    command.Parameters.AddWithValue("@TimeIn", Reservation.TimeIn);

                    command.ExecuteNonQuery();

                    // Mark slot as occupied
                    var updateSlot = new MySqlCommand("UPDATE Slots SET Status='Occupied' WHERE Id=@Id", connection);
                    updateSlot.Parameters.AddWithValue("@Id", Reservation.SlotId);
                    updateSlot.ExecuteNonQuery();
                }

                SuccessMessage = "Reservation created successfully!";
                Response.Redirect("/Reservations/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error creating reservation: " + ex.Message;
            }
        }
        
    }
}