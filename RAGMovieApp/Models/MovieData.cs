namespace RAGMovieApp.Models
{
  public class MovieData
  {
    public static List<Movie> GetMovies()
    {
      return new List<Movie>
      {
        new()
        {
          Title = "The Shawshank Redemption",
          Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
          Reference = "https://en.wikipedia.org/wiki/The_Shawshank_Redemption"
        },
        new()
        {
          Title = "The Matrix",
          Description = "A hacker discovers reality is a simulation and joins a rebellion against machines.",
          Reference = "https://en.wikipedia.org/wiki/The_Matrix"
        },
        new()
        {
          Title = "Inception",
          Description = "A thief who steals corporate secrets through dream-sharing technology is given an inverse task of planting an idea.",
          Reference = "https://en.wikipedia.org/wiki/Inception"
        },
        new()
        {
          Title = "Interstellar",
          Description = "A team travels through a wormhole in space to ensure humanity's survival.",
          Reference = "https://en.wikipedia.org/wiki/Interstellar_(film)"
        }, new()
        {
          Title = "The Godfather",
          Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
          Reference = "https://en.wikipedia.org/wiki/The_Godfather"
        },
        new()
        {
          Title = "Pulp Fiction",
          Description = "The lives of two mob hitmen, a boxer, a gangster's wife, and a pair of diner bandits intertwine in four tales of violence and redemption.",
          Reference = "https://en.wikipedia.org/wiki/Pulp_Fiction"
        },
        new()
        {
          Title = "The Dark Knight",
          Description = "Batman faces the Joker, a criminal mastermind who seeks to create chaos in Gotham City.",
          Reference = "https://en.wikipedia.org/wiki/The_Dark_Knight_(film)"
        },
        new()
        {
          Title = "Forrest Gump",
          Description = "The presidencies of Kennedy and Johnson, the Vietnam War, and other historical events unfold from the perspective of an Alabama man with a low IQ.",
          Reference = "https://en.wikipedia.org/wiki/Forrest_Gump"
        },
        new()
        {
          Title = "Fight Club",
          Description = "An insomniac office worker and a soap maker form an underground fight club that evolves into something much more.",
          Reference = "https://en.wikipedia.org/wiki/Fight_Club"
        },
        new()
        {
          Title = "The Lord of the Rings: The Fellowship of the Ring",
          Description = "A meek Hobbit and eight companions set out on a journey to destroy a powerful ring and save Middle-earth.",
          Reference = "https://en.wikipedia.org/wiki/The_Lord_of_the_Rings:_The_Fellowship_of_the_Ring"
        },
        new()
        {
          Title = "The Lord of the Rings: The Two Towers",
          Description = "Frodo and Sam continue their journey to destroy the One Ring, while Aragorn, Legolas, and Gimli fight to defend Middle-earth.",
          Reference = "https://en.wikipedia.org/wiki/The_Lord_of_the_Rings:_The_Two_Towers"
        },
        new()
        {
          Title = "The Lord of the Rings: The Return of the King",
          Description = "The final confrontation between the forces of good and evil fighting for control of Middle-earth.",
          Reference = "https://en.wikipedia.org/wiki/The_Lord_of_the_Rings:_The_Return_of_the_King"
        },
        new()
        {
          Title = "The Lion King",
          Description = "A young lion prince flees his kingdom after the murder of his father, only to learn the true meaning of responsibility and bravery.",
          Reference = "https://en.wikipedia.org/wiki/The_Lion_King"
        }
      };
    }
  }
}