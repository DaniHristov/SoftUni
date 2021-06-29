namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(x => ids.Contains(x.Id))
                .Select(x => new
            {
                Id = x.Id,
                Name = x.FullName,
                CellNumber = x.Cell.CellNumber,
                Officers = x.PrisonerOfficers.Select(po => new
                {
                    OfficerName = po.Officer.FullName,
                    Department = po.Officer.Department.Name
                })
                .OrderBy(x => x.OfficerName)
                .ToList(),

                TotalOfficerSalary = x.PrisonerOfficers
                .Sum(x => x.Officer.Salary)
                .ToString("F2"),

            })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToList();

            var result = JsonConvert.SerializeObject(prisoners,Formatting.Indented);
            return result;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            throw new NotImplementedException();
        }
    }
}