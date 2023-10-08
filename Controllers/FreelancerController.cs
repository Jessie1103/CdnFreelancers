using CdnFreelancers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json.Serialization;

namespace CdnFreelancers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public FreelancerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllFreelancers")]
        public IActionResult GetAllFreelancers()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("FreelancerAppCon").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Freelancer", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            if (dataTable.Rows.Count > 0)
            {
                List<Freelancer> freelancerList = new List<Freelancer>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Freelancer freelancer = new Freelancer();
                    freelancer.id = Convert.ToInt32(dataTable.Rows[i]["id"]);
                    freelancer.username = Convert.ToString(dataTable.Rows[i]["username"]);
                    freelancer.mail = Convert.ToString(dataTable.Rows[i]["mail"]);
                    freelancer.phoneNum = Convert.ToString(dataTable.Rows[i]["phoneNum"]);
                    freelancer.skillSets = Convert.ToString(dataTable.Rows[i]["skillSets"]);
                    freelancer.hobby = Convert.ToString(dataTable.Rows[i]["hobby"]);
                    freelancerList.Add(freelancer);
                }
                return Ok(freelancerList);
            }
            else
            {
                return NotFound("No data found");
            }
        }

        [HttpGet]
        [Route("GetFreelancerById/{id}")]
        public IActionResult GetFreelancerById(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("FreelancerAppCon").ToString());
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM Freelancer WHERE id = {id}", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            if (dataTable.Rows.Count > 0)
            {
                Freelancer freelancer = new Freelancer();
                freelancer.id = Convert.ToInt32(dataTable.Rows[0]["id"]);
                freelancer.username = Convert.ToString(dataTable.Rows[0]["username"]);
                freelancer.mail = Convert.ToString(dataTable.Rows[0]["mail"]);
                freelancer.phoneNum = Convert.ToString(dataTable.Rows[0]["phoneNum"]);
                freelancer.skillSets = Convert.ToString(dataTable.Rows[0]["skillSets"]);
                freelancer.hobby = Convert.ToString(dataTable.Rows[0]["hobby"]);
                return Ok(freelancer);
            }
            else
            {
                return NotFound("Freelancer not found");
            }
        }

        [HttpPost]
        [Route("CreateFreelancer")]
        public IActionResult CreateFreelancer(Freelancer newFreelancer)
        {
            // Check if required fields are not empty
            if (string.IsNullOrWhiteSpace(newFreelancer.username) ||
                string.IsNullOrWhiteSpace(newFreelancer.mail) ||
                string.IsNullOrWhiteSpace(newFreelancer.phoneNum) ||
                string.IsNullOrWhiteSpace(newFreelancer.skillSets))
            {
                return BadRequest("All required fields (username, mail, phoneNum, skillSets) must be provided and cannot be empty.");
            }

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("FreelancerAppCon").ToString());
            SqlCommand cmd = new SqlCommand("INSERT INTO Freelancer (username, mail, phoneNum, skillSets, hobby) VALUES (@username, @mail, @phoneNum, @skillSets, @hobby)", con);
            cmd.Parameters.AddWithValue("@username", newFreelancer.username);
            cmd.Parameters.AddWithValue("@mail", newFreelancer.mail);
            cmd.Parameters.AddWithValue("@phoneNum", newFreelancer.phoneNum);
            cmd.Parameters.AddWithValue("@skillSets", newFreelancer.skillSets);
            cmd.Parameters.AddWithValue("@hobby", newFreelancer.hobby);

            con.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                return Ok("Freelancer created successfully");
            }
            else
            {
                return BadRequest("Failed to create freelancer");
            }
        }

        [HttpPut]
        [Route("UpdateFreelancer/{id}")]
        public IActionResult UpdateFreelancer(int id, Freelancer newFreelancer)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("FreelancerAppCon").ToString());
            SqlCommand cmd = new SqlCommand("SELECT * FROM Freelancer WHERE id = @id", con);
            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                con.Close();
                return NotFound("Freelancer not found");
            }

            reader.Close();

            // Fetch the existing freelancer data
            cmd.CommandText = "SELECT * FROM Freelancer WHERE id = @id";
            SqlDataReader existingData = cmd.ExecuteReader();

            if (existingData.Read())
            {
                // Check and update each field if it's not an empty string
                string updatedUsername = string.IsNullOrEmpty(newFreelancer.username) ? existingData["username"].ToString() : newFreelancer.username;
                string updatedMail = string.IsNullOrEmpty(newFreelancer.mail) ? existingData["mail"].ToString() : newFreelancer.mail;
                string updatedPhoneNum = string.IsNullOrEmpty(newFreelancer.phoneNum) ? existingData["phoneNum"].ToString() : newFreelancer.phoneNum;
                string updatedSkillSets = string.IsNullOrEmpty(newFreelancer.skillSets) ? existingData["skillSets"].ToString() : newFreelancer.skillSets;
                string updatedHobby = string.IsNullOrEmpty(newFreelancer.hobby) ? existingData["hobby"].ToString() : newFreelancer.hobby;

                // Update the freelancer with the new data
                cmd.CommandText = "UPDATE Freelancer SET username = @username, mail = @mail, phoneNum = @phoneNum, skillSets = @skillSets, hobby = @hobby WHERE id = @id";
                cmd.Parameters.AddWithValue("@username", updatedUsername);
                cmd.Parameters.AddWithValue("@mail", updatedMail);
                cmd.Parameters.AddWithValue("@phoneNum", updatedPhoneNum);
                cmd.Parameters.AddWithValue("@skillSets", updatedSkillSets);
                cmd.Parameters.AddWithValue("@hobby", updatedHobby);

                existingData.Close();

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok("Freelancer updated successfully");
                }
                else
                {
                    return NotFound("Freelancer not found");
                }
            }

            existingData.Close();
            con.Close();
            return BadRequest("Invalid data provided");
        }

        [HttpDelete]
        [Route("DeleteFreelancer/{id}")]
        public IActionResult DeleteFreelancer(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("FreelancerAppCon").ToString());
            SqlCommand cmd = new SqlCommand("DELETE FROM Freelancer WHERE id = @id", con);
            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                return Ok("Freelancer deleted successfully");
            }
            else
            {
                return NotFound("Freelancer not found");
            }
        }
    }
}
