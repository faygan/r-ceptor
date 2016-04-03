using System;
using System.Collections.Generic;
using System.Linq;
using Service.PersonelService.Models;

namespace Service.PersonelService.App_Data
{
    public static class PersonDataProvider
    {
        public static List<PersonDto> PersonData = new List<PersonDto>(
            new[]
            {
                new PersonDto {PersonId = 1, Name = "Ira Kollar", DeptId = 1, DeptName = "Accounting"},
                new PersonDto {PersonId = 2, Name = "Jacqueline Pagano", DeptId = 1, DeptName = "Accounting"},
                new PersonDto {PersonId = 3, Name = "Wilton Parodi", DeptId = 1, DeptName = "Accounting"},
                new PersonDto {PersonId = 4, Name = "Jeanne Betty", DeptId = 1, DeptName = "Accounting"},
                new PersonDto {PersonId = 5, Name = "Erlene Venne", DeptId = 2, IsActive = false, DeptName = "Human Resources"},
                new PersonDto {PersonId = 6, Name = "Isaias Stamey", DeptId = 2, IsActive = true, DeptName = "Human Resources"},
                new PersonDto {PersonId = 7, Name = "Andrea Smith", DeptId = 2, IsActive = false, DeptName = "Human Resources"},
                new PersonDto {PersonId = 8, Name = "Charla Lorusso", DeptId = 2, IsActive = false, DeptName = "Human Resources"},
                new PersonDto {PersonId = 9, Name = "Andrea Valley", DeptId = 2, IsActive = true, DeptName = "Human Resources"},
                new PersonDto {PersonId = 10, Name = "Liz Gregor", DeptId = 3, DeptName = "Business Production"},
                new PersonDto {PersonId = 11, Name = "Woodrow Harrow", DeptId = 3, DeptName = "Business Production"},
                new PersonDto {PersonId = 12, Name = "Zoe Duden", DeptId = 3, DeptName = "Business Production"},
                new PersonDto {PersonId = 13, Name = "Marcy Kerby", DeptId = 3, DeptName = "Business Production"},
                new PersonDto {PersonId = 14, Name = "Ricki Weese", DeptId = 3, DeptName = "Business Production"},
                new PersonDto {PersonId = 15, Name = "Lazaro Talor", DeptId = 3, DeptName = "Business Production"},
                new PersonDto {PersonId = 16, Name = "Ivan Stagg", DeptId = 4,  DeptName = "Information Technology"},
                new PersonDto {PersonId = 17, Name = "Emily Deluca", DeptId = 4 ,DeptName = "Information Technology"},
                new PersonDto {PersonId = 18, Name = "Chasidy Otwell", DeptId = 4, DeptName = "Information Technology"},
                new PersonDto {PersonId = 19, Name = "Maurita Vicario", DeptId = 4, DeptName = "Information Technology"},
                new PersonDto {PersonId = 20, Name = "Barbra Shear", DeptId = 4, DeptName = "Information Technology"},
                new PersonDto {PersonId = 21, Name = "Kimberli Alleman", DeptId = 4, DeptName = "Information Technology"},
                new PersonDto {PersonId = 22, Name = "Maurita Alleman", DeptId = 4,  DeptName = "Information Technology"}
            });

        public static List<PersonPayInfoDto> PersonPaymentData = new List<PersonPayInfoDto>(
            new[]
            {
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 1), PayTotal = 1500m, PaymentDate = new DateTime(2016,2,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 2), PayTotal = 2500m, PaymentDate = new DateTime(2016,1,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 3), PayTotal = 750m, PaymentDate =  new DateTime(2015,8,5)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 4), PayTotal = 15500m, PaymentDate = new DateTime(2016,6,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 5), PayTotal = 6500m, PaymentDate = new DateTime(2016,2,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 6), PayTotal = 3600m, PaymentDate = new DateTime(2016,2,1)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 7), PayTotal = 2500m, PaymentDate = new DateTime(2016,2,11)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 8), PayTotal = 7500m, PaymentDate = new DateTime(2016,2,18)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 9), PayTotal = 9500m, PaymentDate = new DateTime(2016,5,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 10), PayTotal = 6500m, PaymentDate = new DateTime(2019,2,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 11), PayTotal = 1000m, PaymentDate = new DateTime(2016,2,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 12), PayTotal = 300m, PaymentDate = new DateTime(2018,3,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 13), PayTotal = 500m, PaymentDate = new DateTime(2017,4,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 14), PayTotal = 0m, PaymentDate = new DateTime(2016,9,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 15), PayTotal = 500m, PaymentDate = new DateTime(2016,10,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 16), PayTotal = 0m, PaymentDate = new DateTime(2016,11,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 17), PayTotal = 960m, PaymentDate = new DateTime(2016,4,15)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 18), PayTotal = 800m, PaymentDate = new DateTime(2016,6,10)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 19), PayTotal = 100m, PaymentDate = new DateTime(2016,8,12)},
                new PersonPayInfoDto {Person = PersonData.FirstOrDefault(f => f.PersonId == 20), PayTotal = 11500m, PaymentDate = new DateTime(2016,6,13)}
            });

    }
}