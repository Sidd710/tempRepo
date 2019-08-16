using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UpdateInstallerProject
{
    public static class InstallerProjectUpdater
    {
        static Regex productVersionRegex = new Regex("\"ProductVersion\" = \"8:(?<Version>\\d+(\\.\\d+)+)\"");
        static Regex productCodeRegex = new Regex("\"ProductCode\" = \"8:(?<Guid>\\{.+\\})\"");
        static Regex packageCodeRegex = new Regex("\"PackageCode\" = \"8:(?<Guid>\\{.+\\})\"");

        const string VERSION_GROUP = "Version";
        const string GUID_GROUP = "Guid";

        public static void UpdateInstallerProject(string projectFilename, string version)
        {
            if (!File.Exists(projectFilename))
            {
                Console.Error.WriteLine("File '{0}' does not exist", projectFilename);
                Environment.Exit(1);
            }

            // Open project file
            string projectContent = File.ReadAllText(projectFilename, Encoding.UTF8);

            string currentVersion = getValue(projectContent, productVersionRegex, VERSION_GROUP);

            projectContent = projectContent.ReplaceValue(productVersionRegex, VERSION_GROUP, version);
            // Generate new guid for ProductCode
            string newProductCode = Guid.NewGuid().ToString("B").ToUpper();

            // Update ProductCode
            projectContent = projectContent.ReplaceValue(productCodeRegex, GUID_GROUP, newProductCode);

            // Generate new guid for PackageCode
            string newPackageCode = Guid.NewGuid().ToString("B").ToUpper();

            // Update PackageCode
            projectContent = projectContent.ReplaceValue(packageCodeRegex, GUID_GROUP, newPackageCode);

            // Save project file
            File.WriteAllText(projectFilename, projectContent, Encoding.UTF8);

            Console.WriteLine("Updated installer project Version : {0} ProductCode : {1} PackageCode : {2}", version, newProductCode, newPackageCode);
        }

        /// <summary>
        /// Parses a value found with a regex.
        /// Example:
        /// "ProductVersion" = "8:2.1.2"
        /// will return 2.1.2
        /// </summary>
        /// <param name="content"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        private static string getValue(string content, Regex regex, string groupName)
        {
            string value = null;
            var match = regex.Match(content);
            if (match.Success)
            {
                Group group = match.Groups[groupName];
                if (group.Success)
                {
                    value = group.Value;
                }
            }

            return value;
        }

        public static string ReplaceValue(this string input, Regex regex, string groupName, string newValue)
        {
            var match = regex.Match(input);
            string updatedContent = input;

            if (match.Success)
            {
                Group group = match.Groups[groupName];
                if (group.Success)
                {
                    updatedContent = input.Replace(group.Index, group.Length, newValue);
                }
            }

            return updatedContent;
        }

        public static string Replace(this string input, int startIndex, int length, string replacementValue)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(input.Substring(0, startIndex));
            stringBuilder.Append(replacementValue);
            stringBuilder.Append(input.Substring(startIndex + length));

            return stringBuilder.ToString();
        }
    }
}
