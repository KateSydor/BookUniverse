namespace BookUniverse.BLL.DTOs
{

	public class AddBookDto
	{
		public string Title { get; set; }

		public string Description { get; set; }

		public string Author { get; set; }

		public string CategoryName { get; set; }

		public int NumberOfPages { get; set; }

		public string Path { get; set; }
	}
}
