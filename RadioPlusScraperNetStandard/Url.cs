﻿using System;
using System.Linq;

namespace WebScrapingProject
{
    public static class Url
    {
        private static string InternalCombine(string source, string dest)
        {
            // If the source is null or white space retune the dest only
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentException("Cannot be null or white space", "source");

            if (string.IsNullOrWhiteSpace(dest)) throw new ArgumentException("Cannot be null or white space", "dest");

            source = source.TrimEnd('/', '\\');
            dest = dest.TrimStart('/', '\\');

            return string.Format("{0}/{1}", source, dest);
            // return $"{source}/{dest}"; // c# 6.0 string interpolation
        }

        public static string Combine(string source, params string[] args)
        {
            return args.Aggregate(source, InternalCombine);
        }
    }
}