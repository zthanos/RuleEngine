namespace RuleEngineTester.RuleEngine;

public class Customer : IRuleApplicable
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string HomeAddress { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }   
    public string PhoneNumber { get; set; }
    public string Address { get; set; } 
    public string City { get; set; }    
    public string Region { get; set; }  
    public string PostalCode { get; set; }  
    public string Country { get; set; } 
    public int Age { get; set; }
    public bool IsKYCValid { get; set; }
}


