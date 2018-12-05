using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
    public class Item
    {
        private string _description;
        private int _id;
        private DateTime _dueDate = new DateTime();

        public Item(string description, int id = 0, DateTime duedate = new DateTime())
        {
            _description = description;
            _id = id;
            _dueDate = duedate;
        }
        public override bool Equals(System.Object otherItem)
        {
            if (!(otherItem is Item))
            {
                return false;
            }
            else
            {
                Item newItem = (Item)otherItem;
                bool idEquality = (this.GetId() == newItem.GetId());
                bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
                bool dateEquality = (this.GetDueDate() == newItem.GetDueDate());
                return (idEquality && descriptionEquality && dateEquality);
            }
        }
        public DateTime GetDueDate()
        {
            return _dueDate;
        }
        public string GetDescription()
        {
            return _description;
        }
        public void SetDueDate(DateTime dueDate)
        {
            _dueDate = dueDate;
        }

        public void SetDescription(string newDescription)
        {
            _description = newDescription;
        }

        // public int GetId()
        // {
        //   return _id;
        // }

        public static List<Item> GetAll()
        {
            List<Item> allItems = new List<Item> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM items;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int itemId = rdr.GetInt32(0);
                string itemDescription = rdr.GetString(1);
                DateTime itemDueDate = rdr.GetDateTime(2);
                Item newItem = new Item(itemDescription, itemId, itemDueDate);
                allItems.Add(newItem);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allItems;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM items;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO items (description, dueDate) VALUES (@ItemDescription, @ItemDueDate);";
            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@ItemDescription";
            description.Value = this._description;
            MySqlParameter dueDate = new MySqlParameter();
            dueDate.ParameterName = "@ItemDueDate";
            dueDate.Value = this._dueDate;
            cmd.Parameters.Add(description);
            cmd.Parameters.Add(dueDate);
            cmd.ExecuteNonQuery();
            _id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Item Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM `items` WHERE id = @thisId;";
            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int itemId = 0;
            string itemDescription = "";
            DateTime itemDueDate = new DateTime();
            while (rdr.Read())
            {
                itemId = rdr.GetInt32(0);
                itemDescription = rdr.GetString(1);
                itemDueDate = rdr.GetDateTime(2);
            }
            Item foundItem = new Item(itemDescription, itemId, itemDueDate);  // This line is new!
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundItem;
        }

        public int GetId()
        {
            return _id;
        }

        public void Edit(string newDescription)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@newDescription";
            description.Value = newDescription;
            cmd.Parameters.Add(description);
            cmd.ExecuteNonQuery();
            _description = newDescription; // <--- This line is new!
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}