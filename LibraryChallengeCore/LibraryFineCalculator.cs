using System;
using System.Collections.Generic;

namespace LibraryChallengeCore
{
    public class LibraryBookFineCalculator : ILibraryBookFineCalculator
    {
        /// <summary>
        /// Based on todays date calculate the fine for each book and return the total fine due for all of the books.
        /// Some books may not be currently checked out or may not be overdue
        /// 
        /// Rules
        /// 
        /// The fine for an individual book is calculated using the following rule 
        /// 
        /// 1. The library is closed on Sundays so no fees are calculated for Sundays
        /// 2. For days 1 - 10 the fine per day is 0.10
        /// 3. For days 11 - 20 the fine per day is 0.13
        /// 4. For days 21 - 50 the fine per day is 0.17
        /// 5. For days 51+ is 1% of the total fine increasing by 0.3% every day 
    	///     - e.g. day 51 1% of the current total, day 52 1.3% of the current total, day 53 1.6% for the current total
	    ///     - sundays still result in a 0 fine, but do increase the percentage amount
        /// 6. Wednesdays are "double fine" day. If the current day is a Wednesday then the fine for that day is double. Double fine day does not apply once the book is in the 51+ days range
        /// 7. If the book is "non-fiction" then is gets a 25% discount on its total fine
        ///  
        /// </summary>
        /// <param name="today">The date to check the books against to calculate the fine</param>
        /// <param name="books">A list of library books</param>
        /// <returns></returns>
        public decimal CalculateTotalFine(DateTime today, List<ILibraryBook> books)
        {
            decimal totalFine = 0;

            foreach(ILibraryBook _book in books)
            {
                if (_book.DueDate.HasValue && _book.DueDate.Value < today)
                {
                    decimal _tempFine = 0;
                    int tempDays = 0;

                    TimeSpan span = today - _book.DueDate.Value;

                    int totalDays = span.Days;
                    // 1-10
                    if (totalDays > 10)
                    {
                        tempDays = 10;
                    }
                    else
                    {
                        tempDays = totalDays;
                    }

                    _tempFine = (decimal)(0.10) * (tempDays + CountDays(DayOfWeek.Wednesday, today.AddDays(-tempDays + 1), today) - CountDays(DayOfWeek.Sunday, today.AddDays(-tempDays + 1), today));

                    totalDays -= tempDays;
                    //11-20
                    if (totalDays > 0)
                    {
                        if (totalDays > 10)
                        {
                            tempDays = 10;
                        }
                        else
                        {
                            tempDays = totalDays;
                        }

                        _tempFine += (decimal)(0.13) * (tempDays + CountDays(DayOfWeek.Wednesday, today.AddDays(-9 - tempDays), today.AddDays(-10)) - CountDays(DayOfWeek.Sunday, today.AddDays(-9 - tempDays), today.AddDays(-10)));


                        totalDays -= tempDays;
                        //21-50
                        if (totalDays > 0)
                        {
                            if (totalDays > 30)
                            {
                                tempDays = 30;
                            }
                            else
                            {
                                tempDays = totalDays;
                            }

                            _tempFine += (decimal)(0.17) * (tempDays + CountDays(DayOfWeek.Wednesday, today.AddDays(-19 - tempDays), today.AddDays(-20)) - CountDays(DayOfWeek.Sunday, today.AddDays(-19 - tempDays), today.AddDays(-20)));

                            totalDays -= tempDays;
                            // >50
                            if (totalDays > 0)
                            {
                                ///// For days 51+ is 1% of the total fine increasing by 0.3% every day 
                                decimal fine_percentage = 1;
                                for (int i = 0; i < totalDays; i++)
                                {
                                    DateTime dt = today.AddDays(-50 - i);
                                    if (dt.DayOfWeek != DayOfWeek.Sunday)
                                        _tempFine = (decimal)(_tempFine * (100 + fine_percentage) / 100);
                                    fine_percentage += (decimal)(0.3);
                                }
                            }

                        }
                    }
                    /*    if (_book.Category == LibraryBookCategory.Scifi)
                           totalFine += _tempFine;
                       else
                       {
                           totalFine += _tempFine * (decimal)(0.75);
                       }*/
                    if (_book.Category == LibraryBookCategory.Biography)
                        totalFine += _tempFine * (decimal)(0.75);
                    else
                    {
                        totalFine += _tempFine;
                    }
                }


            }
            
            return Math.Truncate(100 * totalFine) / 100;
        }

        static int CountDays(DayOfWeek day, DateTime start, DateTime end)
        {
            TimeSpan ts = end - start;                       // Total duration
            int count = (int)Math.Floor(ts.TotalDays / 7);   // Number of whole weeks
            int remainder = (int)(ts.TotalDays % 7);         // Number of remaining days
            int sinceLastDay = (int)(end.DayOfWeek - day);   // Number of days since last [day]
            if (sinceLastDay < 0) sinceLastDay += 7;         // Adjust for negative days since last [day]

            // If the days in excess of an even week are greater than or equal to the number days since the last [day], then count this one, too.
            if (remainder >= sinceLastDay) count++;

            return count;
        }
    }
}