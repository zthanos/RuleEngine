Rule Name: "isPremiumRule"

Applies to: "SomeContext"

When: 
    & CustomerSince LessThan 2009-01-01

Then:
    Set IsPremium to true


Rule Name: "amountRule"

Applies to: "SomeContext"

When: 
    & (Age > 18 and HomeAddress isNotNull) or Email notEmpty

    & HomeAddress notEmpty

    & PurchaseAmount > 150

    & Email notEmpty

Then:
    Set Discount to Discount + 5
    Set Total to PurchaseAmount * (1 - Discount / 100)
     


Rule Name: "premiumDiscountRule"

Applies to: "SomeContext"

When: 
    & (Age > 18 and HomeAddress isNotNull) or Email notEmpty

    & HomeAddress notEmpty

    | Email notEmpty

Then:
    Set Discount to Discount + 10
    Set Total to PurchaseAmount * (1 - Discount / 100)









