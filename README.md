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

## /ShipRec/Receipts
Shows past receipts submitted by ShipRec users.
Allows deletion of erroneous receipts and modifies stock to account for it.

## /Admin/StockManagement
Lists all stock in the system, allowing Admin users to add or edit stock.

## /Admin/RoleManagement
Lists all users in the system. allowing Admin users to edit their roles.


// Just a place to shove ideas until I'm able to actually start working on them

Starship Parts Manufacturer

DIGITAL STOREFRONT
--------------------
Customer Front-End 
    - Storefront
    - Cart
    - Account Details
    - Payment
Shipping/Receiving Back-End
    - Pending Orders (In And Out)
    - Order History
Administrative Back-End
    - Stock Management
    - Order Management (Deletion of faulty receipts, for example)
    - Role Management (except their own)

ROLES: Customer, Shipper/Receiver, Administrator


PAGES
------------------
Storefront:
    - Storefront should work even if the user doesn't have an account.
    - All items should have a price, stock, name, image, and description.
        - Customer should be able to purchase more than the current stock.
        - Selecting more than the current stock should give a warning that excess stock will
            be shipped when it's available.
        - A current stock of less than 0 should be adjusted to 0. (Better for business.)
    - Items should be able to be added to a cart.
    - Items should be filterable by some characteristics.
    - ADMIN: should be able to change prices and descriptions from the storefront

Cart:
    - Cart should display the total and allow the user to add or remove items.
    - Cart should check if you have a coupon code.
    - Cart should allow user to proceed to payment.

Payment:
    - Payment should allow users to get payment information from their account.
    - Payment should allow users to add payment information to their account.
    - After sending payment, the order should be send into the back-end orders system.
    - Payment should automatically automatically adjust stock numbers.

Account Details:
    - AD should let users see their username, email, payment information, and order history
    - AD should let users cancel orders that haven't been fulfilled yet.
    - AD should let users change their username, password, payment information.

Pending Orders:
    - ONLY VISIBLE TO ADMIN AND SHIPPER/RECEIVER
    - PO should show orders that are in the system, sorted as incoming and outgoing.
    - PO allows users to create receipts and shipping lists. These are visible in OH.
    - Partially-completed orders should stay in the system as partially-completed.
    - Receipts should automatically adjust stock numbers.
    - Shipping lists should send an email to the customer (automatically or otherwise)
    - ADMIN: should be able to add or cancel orders.
        - Canceling an outgoing order should automatically adjust stock numbers.
            (Cancelling an incoming order doesn't matter because that stock wasn't
            accounted for to begin with)
    - MAYBE: Include a button to create a receipt without an order.

Order History:
    - ONLY VISIBLE TO ADMIN AND SHIPPER/RECEIVER
    - OH should show orders and any receipts/shipping lists made for them.
    - Users should be able to cancel receipts from here, in case something has gone wrong.
        - Canceling a receipt or shipping list should automatically adjust stock numbers.
        - Canceling a shipping list should send an email to the customer

Stock Management:
    - ONLY VISIBLE TO ADMIN
    - Admin should be able to look at stock in a list and adjust info directly from there.
    - Admin should be able to go to see Stock Details (as per the customer) from SM.

Role Management:
    - ONLY VISIBLE TO ADMIN
    - Admin should be able to assign roles to and remove roles from users. 
    - Admin should not be able to remove the Admin role from users.

Log-In:
    - Should allow people to log-in.
    - MAYBE?? Allow people to reset password using a captcha.

Contact:
    - Should have a page describing the staff. This is a small team.

ALL PAGES:
    - Should be able to log out
    - Should be able to go to Login.
    - Should have access to Contact and Home page.


- Receipts and Shipping Lists should contain:
    - Tracking Numbers/Waybill Numbers
    - Courier
    - Date <Automatic>
    - Order Number <Automatic>
    - SKU No. + Material List <Including Things Still On Backorder>
     
- Stock Items should contain:
    - Name
    - Manufacturer SKU No
    - Storefront ID No
    - Price
    - Description
    - Image (probably placeholder images)
