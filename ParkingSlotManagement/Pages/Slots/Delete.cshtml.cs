using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using ParkingSlotManagement.Models;

namespace ParkingSlotManagement.Pages.Slots
{
    public class Delete : PageModel
    {
        [BindProperty]
        public SlotInfo slot{get; set;} = new SlotInfo();
        public string ErrorMessage{get; set;}="";
        public string SuccessMessage{get; set;} = "";
        public void OnGet(int id)
        {
            try
            {
                string connectionString = "Server=localhost; Port=3306; Database=ParkingDB; Uid=root; Pwd=manager;";
                using(var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM Slots WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id",id);
                    using(var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            slot.id = reader.GetInt32(0);
                            slot.slotNumber = reader.GetString(1);
                            slot.location = reader.GetString(2);
                            slot.status = reader.GetString(3);
                        }
                        else
                        {
                            ErrorMessage = "slot not found";
                        }
                    }
                }               
            }catch(Exception ex)
            {
                ErrorMessage = "Error delete slot" + ex.Message;
            }
        }

        public void OnPost()
        {
            try
            {
                string connectionString = "Server=localhost; Port=3306; Database=ParkingDB; Uid=root; Pwd=manager;";
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("DELETE FROM Slots WHERE id=@id", connection);
                    command.Parameters.AddWithValue("@id", slot.id);

                    command.ExecuteNonQuery();
                }

                SuccessMessage = "Slot deleted successfully!";
                Response.Redirect("/Slots/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error deleting slot: " + ex.Message;
            }
        }
    }
}