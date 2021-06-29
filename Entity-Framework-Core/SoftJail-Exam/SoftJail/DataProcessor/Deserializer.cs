namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using System.Text;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.IO;
    using SoftJail.Data.Models.Enums;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentCells = JsonConvert.DeserializeObject<IEnumerable<ImportJsonDepartmentCellsModel>>(jsonString);
            var sb = new StringBuilder();

            foreach (var depCell in departmentCells)
            {
                if (!IsValid(depCell) || depCell.Cells.Count == 0 || !IsValid(depCell.Cells.Select(x => x.CellNumber)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var currDepartmend = new Department
                {
                    Name = depCell.Name,
                };
                bool cellValidation = true;
                foreach (var cell in depCell.Cells)
                {
                    if (!IsValid(cell))
                    {
                        sb.AppendLine("Invalid Data");
                        cellValidation = false;
                    }
                    var currentCell = new Cell
                    {
                        CellNumber = cell.CellNumber,
                        HasWindow = cell.HasWindow
                    };

                    currDepartmend.Cells.Add(currentCell);
                }
                if (!cellValidation)
                {
                    continue;
                }
                context.Departments.Add(currDepartmend);
                context.SaveChanges();
                sb.AppendLine($"Imported {currDepartmend.Name} with {currDepartmend.Cells.Count} cells");
            }


            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var prisonerMails = JsonConvert.DeserializeObject<IEnumerable<ImportJsonPrisonerMailsModel>>(jsonString);

            foreach (var prisoner in prisonerMails)
            {
                if (!IsValid(prisoner) || !prisoner.Mails.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var releaseDateParse = DateTime
                    .TryParseExact(prisoner.ReleaseDate,
                    "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var rlsDate);

                var incarcerationDateParse = DateTime
                    .TryParseExact(prisoner.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var incDate);

                var currentPrisoner = new Prisoner()
                {
                    Nickname = prisoner.Nickname,
                    FullName = prisoner.FullName,
                    Age = prisoner.Age,
                    ReleaseDate = rlsDate,
                    IncarcerationDate = incDate,
                    Bail = prisoner.Bail,
                    CellId = prisoner.CellId,
                    Mails = prisoner.Mails.Select(x => new Mail
                    {
                        Sender = x.Sender,
                        Address = x.Address,
                        Description = x.Description
                    })
                    .ToList()
                };
                context.Prisoners.Add(currentPrisoner);
                context.SaveChanges();
                sb.AppendLine($"Imported {currentPrisoner.FullName} {currentPrisoner.Age} years old");


            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportXmlPrisonersOfficersModel[]), new XmlRootAttribute("Officers"));
            var officers = (ImportXmlPrisonersOfficersModel[])serializer.Deserialize(new StringReader(xmlString));
            var sb = new StringBuilder();

            foreach (var op in officers)
            {
                if (!IsValid(op))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var currOfficer = new Officer()
                {
                    FullName = op.Name,
                    DepartmentId = op.DepartmentId,
                    Salary = op.Money,
                    Position = Enum.Parse<Position>(op.Position),
                    Weapon = Enum.Parse<Weapon>(op.Weapon),
                    OfficerPrisoners = op.Prisoners.Select(x => new OfficerPrisoner
                    {
                        PrisonerId = x.Id
                    })
                    .ToList()
                };
                context.Officers.Add(currOfficer);
                context.SaveChanges();
                sb.AppendLine($"Imported {currOfficer.FullName} ({currOfficer.OfficerPrisoners.Count} prisoners)");
            }
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}
