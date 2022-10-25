using System.ComponentModel.DataAnnotations;
//We added the above line 
namespace Portfolio.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage ="* Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "* Email is required")]
        //[RegularExpression("the expression", ErrorMessage ="Provide proper email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Subject { get; set; }
        [Required(ErrorMessage = "* Message is required")]
        [UIHint("MultilineText")]//makes a big text area 
        public string Message { get; set; }


    }
}