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
    public class Index : PageModel
    {
         public List<ReservationInfo> listReservations{get; set;} = new List<ReservationInfo>();
         [BindProperty(SupportsGet = true)]
         public string searchText { get; set;}
        public void OnGet()
        {
            try
            {
                string connectionString = "Server=localhost; Port=3306; Database=ParkingDB; Uid=root; Pwd=manager;";
                using(var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Reservations WHERE VehicleNo LIKE @Search OR SlotId = @SlotId";
                    
                    var command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Search","%" + searchText + "%");

                    int slotId;
                    if(int.TryParse(searchText, out slotId))
                    {
                        command.Parameters.AddWithValue("@SlotId",slotId);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@SlotId", -1);
                    }
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ReservationInfo res = new ReservationInfo
                            {
                                Id = reader.GetInt32(0),
                                SlotId = reader.GetInt32(1),
                                VehicleNo = reader.GetString(2),
                                TimeIn = reader.GetDateTime(3),
                                TimeOut = reader.IsDBNull(4) ? null : reader.GetDateTime(4)
                            };
                            listReservations.Add(res);
                        }
                    }
                }                
            }catch(Exception ex)
            {
                Console.WriteLine("Error loading reservations: " + ex.Message);
            }
        }
    }
}