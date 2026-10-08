"# ComIT_FINAL_NeoQuasar_Industries" 

A digital storefront that sells flickership parts and accessories in a science-fantasy world. Expected clients are on the individual scale, though those individuals may be connected to major powers.

# Screens and their Core Functionalities:
## All Screens 
Search Bar: Allows users to search for products from the digital catalog.
**Search Bar has modular search components available to it:**
- Searching a term with no components or in "Quotation Marks" searches the name and description of stock items.
- The m: or m= component filters for products from a specific manufacturer. 
- The cat: or cat= component filters for products in a certain category.
- The p=, p<, p>, p<=, and p>= components filter for products matching a given price range.
- *Example: A search of `CRG m:"Martin Industries" p<5000` returns every item in the catalog made by Martin Industries that costs 5000c or less with CRG in its name or description, which will most likely return a number of replacement parts for CRG systems.*

## /Home/Storefront
Lists products available to purchase.
Allows users to add and remove items from their cart.
Displays the current stock and price of products.

## /Home/Cart
Allows users to review the objects they wish to purchase and calculates import tax based on the user's Locator Code.

## /Home/CheckOut
Allows users to review their account information and select a shipping option before finalizing the order.
Gives users the opportunity to go to their account details to make changes.
Submits the order to the S/R database.

## /Home/Item/<ID>
Allows users to see the details of stock items and add them to their cart from there.
Indicates when an item isn't going to be restocked but remains in the database for backwards compatibility.
(Largely unnecessary at the current scale but important for future potential developments.)

## /Account/Login
Allows users to log-in and stay logged in.
Links to the registration page.

## /Account/Register
Allows user to create accounts containing:
- Username
- Name (First + Last, the separation of which is irrelevant to the program)
- Two forms of contact, of which one must be present:
    - StellarNet Address: Structured as ven0/kari.astera4
    - Sling Name: Structured as StarlitePrincess
- Address
- Locator Code -- the equivalent of what we would call a Postal Code
The program can tell that you haven't structured your sN address correctly, because like emails, it would need to use that if it were actually contacting people.

## /Account/AccountDetails
Shows users their name, contacts, order history, and payment details.
Allows users to change these various details through /Account/Update

## /Account/Cards
Allows users to add payment details to their account.
Payment details have Card Names, Card Numbers, and Card Types. The Card Numbers have structures based on the card types:
- Credit cards are structured like ast0:1234567890 with optional whitespace
- StellarNet Debit cards are structured like as1234-12345 with an unbound number of trailing digits
- Requisition Accounts are structured like RASTERA-ST:123 with an unbound number of initial letters and trailing digits.

## /ShipRec/PendingOrders
Shows incoming and outgoing orders to Shippers and Receivers.
Allows ShipRec users to input receipts for orders in the database.
Color codes outgoing orders depending on the shipping tier of the purchase.
Incoming and Outgoing receipts automatically modify the amount of stock in the database.
*Future iterations would automatically send an email with tracking information to the client.*

## /ShipRec/Receipts
Shows past receipts submitted by ShipRec users.
Allows deletion of erroneous receipts and modifies stock to account for it.

## /Admin/StockManagement
Lists all stock in the system, allowing Admin users to add or edit stock.

## /Admin/RoleManagement
Lists all users in the system. allowing Admin users to edit their roles.


VERSION NOTES:
- add category to stock item pages and make helper methods to convert from category enum to string and back.


TO DO LIST:
- rewrite jquery code to be alpine where possible
- pull code out of models and put it in services. (Nothing should look at Program.Database() anymore because that makes the services less portable)
- clean up controllers so they have the correct data tags
- make stock search sort out items that won't restock and have no stock
- make cart and check out screens update when you push buttons
- remove id dependency from cart and user details
- make carts not crash when you try to purchase from them without a selected payment detail
- make carts work when not signed in (using browser cache, maybe?)
- either allow new blank orders or, you know,  don't. 
- finish making the edit stock page work
- make the register validation compare passwords and also check for sn/sling
- make some method for adding manufacturers
- require authorization so people can't just get pages by typing urls
- load the databases
- add a button to remove an account.

STRETCH GOALS:
- Pageify + sort the info on admin pages
- API access to the NQI inventory