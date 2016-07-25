using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryChallengeCore
{
    public interface IBookCategory
    {
        LibraryBookCategory Category { get; set; }
        string CategoryTitle { get;  }
        int BookQuantity { get;  }
        decimal BookFine { get;  }

        IEnumerable<ILibraryBook> BookList { get; set; }
    }
}
