"# ComIT_FINAL_NeoQuasar_Industries" 
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
