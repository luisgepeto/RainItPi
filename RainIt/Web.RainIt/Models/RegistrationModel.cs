using System.Collections.Generic;
using System.Web.Mvc;
using RainIt.Domain.DTO;

namespace Web.RainIt.Models
{
    public class RegistrationModel
    {
        public Registration Registration { get; set; }
        public SelectList GenderList
        {
            get
            {
                var genderList = new List<string>{"Male", "Female", "Unspecified"};
                return new SelectList(genderList);
            }
        }

        public SelectList CountryList
        {
            get
            {
                var countryList = new List<string>{"Mexico", "Other"};
                return new SelectList(countryList);
            }
        }

        public SelectList StateList
        {
            get
            {
                var stateList = new List<string>{"Nuevo Leon", "Mexico", "Other"};
                return new SelectList(stateList);
            }
        }

        public SelectList CityList
        {
            get
            {
                var countryList = new List<string>{"Monterrey", "Toluca", "Other"};
                return new SelectList(countryList);
            }
        }
    }
}