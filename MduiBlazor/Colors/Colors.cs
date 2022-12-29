using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public abstract class Colors
    {
        public static class Theme
        {
            public static string Primary { get; } = "theme";
            public static string Primary0 { get; } = "theme-50";
            public static string Primary1 { get; } = "theme-100";
            public static string Primary2 { get; } = "theme-200";
            public static string Primary3 { get; } = "theme-300";
            public static string Primary4 { get; } = "theme-400";
            public static string Primary5 { get; } = "theme-500";
            public static string Primary6 { get; } = "theme-600";
            public static string Primary7 { get; } = "theme-700";
            public static string Primary8 { get; } = "theme-800";
            public static string Primary9 { get; } = "theme-900";
            public static string Accent { get; } = "theme-accent";
            public static string Accent1 { get; } = "theme-a100";
            public static string Accent2 { get; } = "theme-a200";
            public static string Accent4 { get; } = "theme-a400";
            public static string Accent7 { get; } = "theme-a700";
        }

        public static class Red
        {
            public static string Primary { get; } = "red";
            public static string Primary0 { get; } = "red-50";
            public static string Primary1 { get; } = "red-100";
            public static string Primary2 { get; } = "red-200";
            public static string Primary3 { get; } = "red-300";
            public static string Primary4 { get; } = "red-400";
            public static string Primary5 { get; } = "red-500";
            public static string Primary6 { get; } = "red-600";
            public static string Primary7 { get; } = "red-700";
            public static string Primary8 { get; } = "red-800";
            public static string Primary9 { get; } = "red-900";
            public static string Accent { get; } = "red-accent";
            public static string Accent1 { get; } = "red-a100";
            public static string Accent2 { get; } = "red-a200";
            public static string Accent4 { get; } = "red-a400";
            public static string Accent7 { get; } = "red-a700";
        }

        public static class Pink
        {
            public static string Primary { get; } = "pink";
            public static string Primary0 { get; } = "pink-50";
            public static string Primary1 { get; } = "pink-100";
            public static string Primary2 { get; } = "pink-200";
            public static string Primary3 { get; } = "pink-300";
            public static string Primary4 { get; } = "pink-400";
            public static string Primary5 { get; } = "pink-500";
            public static string Primary6 { get; } = "pink-600";
            public static string Primary7 { get; } = "pink-700";
            public static string Primary8 { get; } = "pink-800";
            public static string Primary9 { get; } = "pink-900";
            public static string Accent { get; } = "pink-accent";
            public static string Accent1 { get; } = "pink-a100";
            public static string Accent2 { get; } = "pink-a200";
            public static string Accent4 { get; } = "pink-a400";
            public static string Accent7 { get; } = "pink-a700";
        }

        public static class Purple
        {
            public static string Primary { get; } = "purple";
            public static string Primary0 { get; } = "purple-50";
            public static string Primary1 { get; } = "purple-100";
            public static string Primary2 { get; } = "purple-200";
            public static string Primary3 { get; } = "purple-300";
            public static string Primary4 { get; } = "purple-400";
            public static string Primary5 { get; } = "purple-500";
            public static string Primary6 { get; } = "purple-600";
            public static string Primary7 { get; } = "purple-700";
            public static string Primary8 { get; } = "purple-800";
            public static string Primary9 { get; } = "purple-900";
            public static string Accent { get; } = "purple-accent";
            public static string Accent1 { get; } = "purple-a100";
            public static string Accent2 { get; } = "purple-a200";
            public static string Accent4 { get; } = "purple-a400";
            public static string Accent7 { get; } = "purple-a700";
        }

        public static class DeepPurple
        {
            public static string Primary { get; } = "deep-purple";
            public static string Primary0 { get; } = "deep-purple-50";
            public static string Primary1 { get; } = "deep-purple-100";
            public static string Primary2 { get; } = "deep-purple-200";
            public static string Primary3 { get; } = "deep-purple-300";
            public static string Primary4 { get; } = "deep-purple-400";
            public static string Primary5 { get; } = "deep-purple-500";
            public static string Primary6 { get; } = "deep-purple-600";
            public static string Primary7 { get; } = "deep-purple-700";
            public static string Primary8 { get; } = "deep-purple-800";
            public static string Primary9 { get; } = "deep-purple-900";
            public static string Accent { get; } = "deep-purple-accent";
            public static string Accent1 { get; } = "deep-purple-a100";
            public static string Accent2 { get; } = "deep-purple-a200";
            public static string Accent4 { get; } = "deep-purple-a400";
            public static string Accent7 { get; } = "deep-purple-a700";
        }

        public static class Indigo
        {
            public static string Primary { get; } = "indigo";
            public static string Primary0 { get; } = "indigo-50";
            public static string Primary1 { get; } = "indigo-100";
            public static string Primary2 { get; } = "indigo-200";
            public static string Primary3 { get; } = "indigo-300";
            public static string Primary4 { get; } = "indigo-400";
            public static string Primary5 { get; } = "indigo-500";
            public static string Primary6 { get; } = "indigo-600";
            public static string Primary7 { get; } = "indigo-700";
            public static string Primary8 { get; } = "indigo-800";
            public static string Primary9 { get; } = "indigo-900";
            public static string Accent { get; } = "indigo-accent";
            public static string Accent1 { get; } = "indigo-a100";
            public static string Accent2 { get; } = "indigo-a200";
            public static string Accent4 { get; } = "indigo-a400";
            public static string Accent7 { get; } = "indigo-a700";
        }

        public static class Blue
        {
            public static string Primary { get; } = "blue";
            public static string Primary0 { get; } = "blue-50";
            public static string Primary1 { get; } = "blue-100";
            public static string Primary2 { get; } = "blue-200";
            public static string Primary3 { get; } = "blue-300";
            public static string Primary4 { get; } = "blue-400";
            public static string Primary5 { get; } = "blue-500";
            public static string Primary6 { get; } = "blue-600";
            public static string Primary7 { get; } = "blue-700";
            public static string Primary8 { get; } = "blue-800";
            public static string Primary9 { get; } = "blue-900";
            public static string Accent { get; } = "blue-accent";
            public static string Accent1 { get; } = "blue-a100";
            public static string Accent2 { get; } = "blue-a200";
            public static string Accent4 { get; } = "blue-a400";
            public static string Accent7 { get; } = "blue-a700";
        }

        public static class LightBlue
        {
            public static string Primary { get; } = "light-blue";
            public static string Primary0 { get; } = "light-blue-50";
            public static string Primary1 { get; } = "light-blue-100";
            public static string Primary2 { get; } = "light-blue-200";
            public static string Primary3 { get; } = "light-blue-300";
            public static string Primary4 { get; } = "light-blue-400";
            public static string Primary5 { get; } = "light-blue-500";
            public static string Primary6 { get; } = "light-blue-600";
            public static string Primary7 { get; } = "light-blue-700";
            public static string Primary8 { get; } = "light-blue-800";
            public static string Primary9 { get; } = "light-blue-900";
            public static string Accent { get; } = "light-blue-accent";
            public static string Accent1 { get; } = "light-blue-a100";
            public static string Accent2 { get; } = "light-blue-a200";
            public static string Accent4 { get; } = "light-blue-a400";
            public static string Accent7 { get; } = "light-blue-a700";
        }

        public static class Cyan
        {
            public static string Primary { get; } = "cyan";
            public static string Primary0 { get; } = "cyan-50";
            public static string Primary1 { get; } = "cyan-100";
            public static string Primary2 { get; } = "cyan-200";
            public static string Primary3 { get; } = "cyan-300";
            public static string Primary4 { get; } = "cyan-400";
            public static string Primary5 { get; } = "cyan-500";
            public static string Primary6 { get; } = "cyan-600";
            public static string Primary7 { get; } = "cyan-700";
            public static string Primary8 { get; } = "cyan-800";
            public static string Primary9 { get; } = "cyan-900";
            public static string Accent { get; } = "cyan-accent";
            public static string Accent1 { get; } = "cyan-a100";
            public static string Accent2 { get; } = "cyan-a200";
            public static string Accent4 { get; } = "cyan-a400";
            public static string Accent7 { get; } = "cyan-a700";
        }

        public static class Teal
        {
            public static string Primary { get; } = "teal";
            public static string Primary0 { get; } = "teal-50";
            public static string Primary1 { get; } = "teal-100";
            public static string Primary2 { get; } = "teal-200";
            public static string Primary3 { get; } = "teal-300";
            public static string Primary4 { get; } = "teal-400";
            public static string Primary5 { get; } = "teal-500";
            public static string Primary6 { get; } = "teal-600";
            public static string Primary7 { get; } = "teal-700";
            public static string Primary8 { get; } = "teal-800";
            public static string Primary9 { get; } = "teal-900";
            public static string Accent { get; } = "teal-accent";
            public static string Accent1 { get; } = "teal-a100";
            public static string Accent2 { get; } = "teal-a200";
            public static string Accent4 { get; } = "teal-a400";
            public static string Accent7 { get; } = "teal-a700";
        }

        public static class Green
        {
            public static string Primary { get; } = "green";
            public static string Primary0 { get; } = "green-50";
            public static string Primary1 { get; } = "green-100";
            public static string Primary2 { get; } = "green-200";
            public static string Primary3 { get; } = "green-300";
            public static string Primary4 { get; } = "green-400";
            public static string Primary5 { get; } = "green-500";
            public static string Primary6 { get; } = "green-600";
            public static string Primary7 { get; } = "green-700";
            public static string Primary8 { get; } = "green-800";
            public static string Primary9 { get; } = "green-900";
            public static string Accent { get; } = "green-accent";
            public static string Accent1 { get; } = "green-a100";
            public static string Accent2 { get; } = "green-a200";
            public static string Accent4 { get; } = "green-a400";
            public static string Accent7 { get; } = "green-a700";
        }

        public static class LightGreen
        {
            public static string Primary { get; } = "light-green";
            public static string Primary0 { get; } = "light-green-50";
            public static string Primary1 { get; } = "light-green-100";
            public static string Primary2 { get; } = "light-green-200";
            public static string Primary3 { get; } = "light-green-300";
            public static string Primary4 { get; } = "light-green-400";
            public static string Primary5 { get; } = "light-green-500";
            public static string Primary6 { get; } = "light-green-600";
            public static string Primary7 { get; } = "light-green-700";
            public static string Primary8 { get; } = "light-green-800";
            public static string Primary9 { get; } = "light-green-900";
            public static string Accent { get; } = "light-green-accent";
            public static string Accent1 { get; } = "light-green-a100";
            public static string Accent2 { get; } = "light-green-a200";
            public static string Accent4 { get; } = "light-green-a400";
            public static string Accent7 { get; } = "light-green-a700";
        }

        public static class Lime
        {
            public static string Primary { get; } = "lime";
            public static string Primary0 { get; } = "lime-50";
            public static string Primary1 { get; } = "lime-100";
            public static string Primary2 { get; } = "lime-200";
            public static string Primary3 { get; } = "lime-300";
            public static string Primary4 { get; } = "lime-400";
            public static string Primary5 { get; } = "lime-500";
            public static string Primary6 { get; } = "lime-600";
            public static string Primary7 { get; } = "lime-700";
            public static string Primary8 { get; } = "lime-800";
            public static string Primary9 { get; } = "lime-900";
            public static string Accent { get; } = "lime-accent";
            public static string Accent1 { get; } = "lime-a100";
            public static string Accent2 { get; } = "lime-a200";
            public static string Accent4 { get; } = "lime-a400";
            public static string Accent7 { get; } = "lime-a700";
        }

        public static class Yellow
        {
            public static string Primary { get; } = "yellow";
            public static string Primary0 { get; } = "yellow-50";
            public static string Primary1 { get; } = "yellow-100";
            public static string Primary2 { get; } = "yellow-200";
            public static string Primary3 { get; } = "yellow-300";
            public static string Primary4 { get; } = "yellow-400";
            public static string Primary5 { get; } = "yellow-500";
            public static string Primary6 { get; } = "yellow-600";
            public static string Primary7 { get; } = "yellow-700";
            public static string Primary8 { get; } = "yellow-800";
            public static string Primary9 { get; } = "yellow-900";
            public static string Accent { get; } = "yellow-accent";
            public static string Accent1 { get; } = "yellow-a100";
            public static string Accent2 { get; } = "yellow-a200";
            public static string Accent4 { get; } = "yellow-a400";
            public static string Accent7 { get; } = "yellow-a700";
        }

        public static class Amber
        {
            public static string Primary { get; } = "amber";
            public static string Primary0 { get; } = "amber-50";
            public static string Primary1 { get; } = "amber-100";
            public static string Primary2 { get; } = "amber-200";
            public static string Primary3 { get; } = "amber-300";
            public static string Primary4 { get; } = "amber-400";
            public static string Primary5 { get; } = "amber-500";
            public static string Primary6 { get; } = "amber-600";
            public static string Primary7 { get; } = "amber-700";
            public static string Primary8 { get; } = "amber-800";
            public static string Primary9 { get; } = "amber-900";
            public static string Accent { get; } = "amber-accent";
            public static string Accent1 { get; } = "amber-a100";
            public static string Accent2 { get; } = "amber-a200";
            public static string Accent4 { get; } = "amber-a400";
            public static string Accent7 { get; } = "amber-a700";
        }

        public static class Orange
        {
            public static string Primary { get; } = "orange";
            public static string Primary0 { get; } = "orange-50";
            public static string Primary1 { get; } = "orange-100";
            public static string Primary2 { get; } = "orange-200";
            public static string Primary3 { get; } = "orange-300";
            public static string Primary4 { get; } = "orange-400";
            public static string Primary5 { get; } = "orange-500";
            public static string Primary6 { get; } = "orange-600";
            public static string Primary7 { get; } = "orange-700";
            public static string Primary8 { get; } = "orange-800";
            public static string Primary9 { get; } = "orange-900";
            public static string Accent { get; } = "orange-accent";
            public static string Accent1 { get; } = "orange-a100";
            public static string Accent2 { get; } = "orange-a200";
            public static string Accent4 { get; } = "orange-a400";
            public static string Accent7 { get; } = "orange-a700";
        }

        public static class DeepOrange
        {
            public static string Primary { get; } = "deep-orange";
            public static string Primary0 { get; } = "deep-orange-50";
            public static string Primary1 { get; } = "deep-orange-100";
            public static string Primary2 { get; } = "deep-orange-200";
            public static string Primary3 { get; } = "deep-orange-300";
            public static string Primary4 { get; } = "deep-orange-400";
            public static string Primary5 { get; } = "deep-orange-500";
            public static string Primary6 { get; } = "deep-orange-600";
            public static string Primary7 { get; } = "deep-orange-700";
            public static string Primary8 { get; } = "deep-orange-800";
            public static string Primary9 { get; } = "deep-orange-900";
            public static string Accent { get; } = "deep-orange-accent";
            public static string Accent1 { get; } = "deep-orange-a100";
            public static string Accent2 { get; } = "deep-orange-a200";
            public static string Accent4 { get; } = "deep-orange-a400";
            public static string Accent7 { get; } = "deep-orange-a700";
        }

        public static class Brown
        {
            public static string Primary { get; } = "brown";
            public static string Primary0 { get; } = "brown-50";
            public static string Primary1 { get; } = "brown-100";
            public static string Primary2 { get; } = "brown-200";
            public static string Primary3 { get; } = "brown-300";
            public static string Primary4 { get; } = "brown-400";
            public static string Primary5 { get; } = "brown-500";
            public static string Primary6 { get; } = "brown-600";
            public static string Primary7 { get; } = "brown-700";
            public static string Primary8 { get; } = "brown-800";
            public static string Primary9 { get; } = "brown-900";
        }

        public static class BlueGrey
        {
            public static string Primary { get; } = "blue-grey";
            public static string Primary0 { get; } = "blue-grey-50";
            public static string Primary1 { get; } = "blue-grey-100";
            public static string Primary2 { get; } = "blue-grey-200";
            public static string Primary3 { get; } = "blue-grey-300";
            public static string Primary4 { get; } = "blue-grey-400";
            public static string Primary5 { get; } = "blue-grey-500";
            public static string Primary6 { get; } = "blue-grey-600";
            public static string Primary7 { get; } = "blue-grey-700";
            public static string Primary8 { get; } = "blue-grey-800";
            public static string Primary9 { get; } = "blue-grey-900";
        }
        public static class Grey
        {
            public static string Primary { get; } = "grey";
            public static string Primary0 { get; } = "grey-50";
            public static string Primary1 { get; } = "grey-100";
            public static string Primary2 { get; } = "grey-200";
            public static string Primary3 { get; } = "grey-300";
            public static string Primary4 { get; } = "grey-400";
            public static string Primary5 { get; } = "grey-500";
            public static string Primary6 { get; } = "grey-600";
            public static string Primary7 { get; } = "grey-700";
            public static string Primary8 { get; } = "grey-800";
            public static string Primary9 { get; } = "grey-900";
        }

        public static class Shades
        {
            public static string Black { get; } = "black";
            public static string White { get; } = "white";
            public static string Transparent { get; } = "transparent";
        }
    }
}
