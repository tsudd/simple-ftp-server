using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace ServiceLayer
{
    public class ServiceLayerService
    {
        private readonly DataAccess DAL;

        public ServiceLayerService(Model.DataManagerOptions.ConnectionOptions connectionOptions)
        {
            DAL = new DataAccess(connectionOptions);
        }

        public int GetMaxId()
        {
            return DAL.GetMaxId();
        }
        public PersonGeneral GetAllPersonInfo(int id)
        {
            var person = DAL.GetPersonById(id);
            var personGeneral = LoadPersonData(person);
            return personGeneral;
        }

        public List<PersonGeneral> GetAllPersons()
        {
            var persons = DAL.GetPeople();
            var ans = new List<PersonGeneral>();
            foreach(var person in persons)
            {
                ans.Add(LoadPersonData(person));
            }
            return ans;
        }

        public List<PersonGeneral> GetPeopleInRange(int startInd, int endInd)
        {
            var persons = DAL.GetPeopleRange(startInd, endInd);
            var ans = new List<PersonGeneral>();
            foreach(var person in persons)
            {
                ans.Add(LoadPersonData(person));
            }
            return ans;
        }

        public List<PersonGeneral> GetPeopleJoin()
        {
            return DAL.GetPeopleWithJoin();
        }

        private PersonGeneral LoadPersonData(Person person)
        {
            int id = person.BusinessEntityID;
            var password = DAL.GetPasswordById(id);
            var email = DAL.GetEmailById(id);
            var personPhone = DAL.GetPhoneById(id);
            var address = DAL.GetAddressById(id);
            var personInfo = new PersonGeneral(person, password, email, personPhone, address);
            return personInfo;
        }
    }
}
