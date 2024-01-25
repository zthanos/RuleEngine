Rule Name: "SampleRule"

Applies to: "SomeContext"

When: 
    & (Age > 18 and HomeAddress isNotNull) or Email notEmpty

    & HomeAddress notEmpty

    | Email notEmpty

Then:
    Set Total to PurchaseAmount * (1 - Discount / 100)
    Set Discount to Discount + 5
    Set Discount to 4
    Set Total to PurchaseAmount


Rule Name: "amountRule"

Applies to: "SomeContext"

When: 
    & (Age > 18 and HomeAddress isNotNull) or Email notEmpty

    & HomeAddress notEmpty

    | Email notEmpty

Then:
    Set Total to PurchaseAmount * (1 - Discount / 100)
    Set Discount to Discount + 5
    Set Discount to 4
    Set Total to PurchaseAmount



Rule Name: "premiumDiscountRule"

Applies to: "SomeContext"

When: 
    & (Age > 18 and HomeAddress isNotNull) or Email notEmpty

    & HomeAddress notEmpty

    | Email notEmpty

Then:
    Set Total to PurchaseAmount * (1 - Discount / 100)
    Set Discount to Discount + 5
    Set Discount to 4
    Set Total to PurchaseAmount