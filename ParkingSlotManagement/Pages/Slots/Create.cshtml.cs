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
    public class Create : PageModel
    {
        [BindProperty]
        public SlotInfo slot {get; set;} = new SlotInfo();

        public string ErrorMessage{ get; set;} = "";
        public string SuccessMessage{get; set;} = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Form is invalid";
                return;
            }
            try
            {
                string connectionString = "Server=localhost; Port=3306; Database=ParkingDB; Uid=root; Pwd=manager;";
                using(var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("INSERT INTO Slots (slotNumber, location, status) VALUES (@slotNumber, @location, @status)", connection);
                    command.Parameters.AddWithValue("@slotNumber",slot.slotNumber);
                    command.Parameters.AddWithValue("@location",slot.location);
                    command.Parameters.AddWithValue("@status",slot.status);

                    command.ExecuteNonQuery();
                }
                SuccessMessage = "New Slot added Successfully";
                Response.Redirect("/Slots/Index");
                
            }catch(Exception ex)
            {
                ErrorMessage = "Error adding slot"+ex.Message;
            }
        }
    }
}