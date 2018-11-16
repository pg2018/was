using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using WebAssistedSurvey.Models;

namespace WebAssistedSurvey.Business
{
    internal class CsvExporter
    {
        internal static byte[] GetCsvData(Event @event)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(GetHeaderRow(@event));

            var title = @event.Title;
            var date = @event.StartDateTime.ToShortDateString();
            foreach (var survey in @event.Surveys)
            {
                sb.AppendLine($"{title};{date};{GetValueRow(survey)}");
            }

            return Encoding.Unicode.GetBytes(sb.ToString());
        }

        private static string GetHeaderRow(Event @event)
        {
            var headerRow = "Veranstaltung;Veranstaltungs Datum;";

            var propertyInfos = @event.Surveys.First().GetType().GetProperties().ToList();

            foreach (var property in propertyInfos)
            {
                var customAttributes = property.CustomAttributes.ToList();
                if (customAttributes.Any())
                {
                    foreach (var attribute in customAttributes)
                    {
                        if (attribute.AttributeType != typeof(DisplayAttribute))
                        {
                            continue;
                        }

                        var name = attribute.NamedArguments.FirstOrDefault().TypedValue.Value;
                        headerRow += $"{name};";
                    }
                }
            }

            return headerRow;
        }

        private static string GetValueRow(Survey survey)
        {
            var result = string.Empty;

            var propertyInfos = survey.GetType().GetProperties().ToList();

            foreach (var property in propertyInfos)
            {
                var customAttributes = property.CustomAttributes.ToList();
                if (customAttributes.Any())
                {
                    foreach (var attribute in customAttributes)
                    {
                        if (attribute.AttributeType != typeof(DisplayAttribute))
                        {
                            continue;
                        }

                        var value = property.GetValue(survey);

                        if (value is DateTime time)
                        {
                            result += $"{time};";
                            continue;
                        }

                        if (value is bool newsletter)
                        {
                            result += newsletter ? "Ja;" : "Nein;";
                            continue;
                        }

                        if (value == null)
                        {
                            result += "Keine Angabe;";
                            continue;
                        }
                        else
                        {
                            result += $"{value};";
                        }
                    }
                }
            }

            return result;
        }
    }
}