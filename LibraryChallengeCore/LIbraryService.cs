using System;
using System.Collections.Generic;
using System.Linq;
using LibraryChallengeCore.Data;

namespace LibraryChallengeCore
{
    public class LibraryService
    {
        private readonly IList<ILibraryBook> _books;

        public LibraryService()
        {
            _books = new LibraryData().Books;
        }

        public IList<ILibraryBook> AllBooks()
        {
            return _books;
        }

        public IEnumerable<ILibraryBook> AllBooks(LibraryBookCategory category)
        {
            return _books.Where(lb => lb.Category == category);
        }

        public IEnumerable<ILibraryBook> AllBooksCategorized()
        {
            return _books.OrderBy(lb => lb.Category).ThenBy(lb => lb.Title);
        }

        public IList<IBookCategory> AllBooksByCategory()
        {
            List<IBookCategory> _list = new List<IBookCategory>();
            List<LibraryBookCategory> _categories = _books.Select(x => x.Category).Distinct().ToList();
            foreach (LibraryBookCategory category in _categories)
            {
                BookCategory bc = new BookCategory();
                bc.Category = category;
                bc.BookList = _books.Where(lb => lb.Category == category).OrderBy(lb => lb.Title);
                _list.Add(bc);
            }
            return _list;
        }

        public ILibraryBook Book(Guid bookId)
        {
            return _books.FirstOrDefault(b => b.BookId == bookId);
        }
    }
}
