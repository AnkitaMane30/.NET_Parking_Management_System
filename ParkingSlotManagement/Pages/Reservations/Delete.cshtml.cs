using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace ParkingSlotManagement.Pages.Reservations
{
    public class Delete : PageModel
    {
       public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                string connectionString = "Server=localhost; Port=3306; Database=ParkingDB; Uid=root; Pwd=manager;";
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Get SlotId before deleting
                    int slotId = 0;
                    var getSlot = new MySqlCommand("SELECT SlotId FROM Reservations WHERE Id=@Id", connection);
                    getSlot.Parameters.AddWithValue("@Id", id);
                    var result = getSlot.ExecuteScalar();
                    if (result != null)
                    {
                        slotId = Convert.ToInt32(result);
                    }

                    // Delete reservation
                    var deleteRes = new MySqlCommand("DELETE FROM Reservations WHERE Id=@Id", connection);
                    deleteRes.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = deleteRes.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Free the slot if reservation existed
                        if (slotId > 0)
                        {
                            var updateSlot = new MySqlCommand("UPDATE Slots SET Status='Available' WHERE Id=@Id", connection);
                            updateSlot.Parameters.AddWithValue("@Id", slotId);
                            updateSlot.ExecuteNonQuery();
                        }

                        SuccessMessage = "Reservation deleted successfully!";
                        Response.Redirect("/Reservations/Index");
                    }
                    else
                    {
                        ErrorMessage = "Reservation not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error deleting reservation: " + ex.Message;
            }
        }
    }
}