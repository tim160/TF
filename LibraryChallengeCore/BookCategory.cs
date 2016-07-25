using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryChallengeCore;

namespace LibraryChallengeCore
{
    class BookCategory : IBookCategory
    {

        public string CategoryTitle
        {
            get
            {
                return Category.ToString();
            }
        }
        public LibraryBookCategory Category { get; set; }
        public int BookQuantity { get { return BookList.Count(); } }
        public decimal BookFine
        {
            get
            {
                LibraryBookFineCalculator dd = new LibraryBookFineCalculator();
                return dd.CalculateTotalFine(DateTime.Today, BookList.ToList());
            }
        }
        public IEnumerable<ILibraryBook> BookList { get; set; }

        
    }
}
