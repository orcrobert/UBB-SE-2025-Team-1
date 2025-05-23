﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIApp.Models
{
    public class Brand
    {
        private int _id;
        private string _name;

        public Brand(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Brand name cannot be null or empty", nameof(Name));
                _name = value;
            }
        }
    }
}
