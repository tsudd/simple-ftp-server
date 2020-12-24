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

        public async Task<int> GetMaxIdAsync()
        {
            return await DAL.GetMaxIdAsync();
        }

        public async Task<PersonGeneral> GetAllPersonInfoAsync(int id)
        {
            var person = await DAL.GetPersonByIdAsync(id);
            var personGeneral = await LoadPersonDataAsync(person);
            return personGeneral;
        }

        public async Task<List<PersonGeneral>> GetAllPersonsAsync()
        {
            var persons = await DAL.GetPeopleAsync();
            var ans = new List<PersonGeneral>();
            foreach (var person in persons)
            {
                ans.Add(await LoadPersonDataAsync(person));
            }
            return ans;
        }

        public async Task<List<PersonGeneral>> GetPeopleInRangeAsync(int startInd, int endInd)
        {
            var persons = await DAL.GetPeopleRangeAsync(startInd, endInd);
            var ans = new List<PersonGeneral>();
            foreach (var person in persons)
            {
                ans.Add(await LoadPersonDataAsync(person));
            }
            return ans;
        }

        public async Task<List<PersonGeneral>> GetPeopleJoinAsync()
        {
            return await DAL.GetPeopleWithJoinAsync();
        }

        private async Task<PersonGeneral> LoadPersonDataAsync(Person person)
        {
            int id = person.BusinessEntityID;
            var password = await DAL.GetPasswordByIdAsync(id);
            var email = await DAL.GetEmailByIdAsync(id);
            var personPhone = await DAL.GetPhoneByIdAsync(id);
            var address = await DAL.GetAddressByIdAsync(id);
            var personInfo = new PersonGeneral(person, password, email, personPhone, address);
            return personInfo;
        }
    }
}
