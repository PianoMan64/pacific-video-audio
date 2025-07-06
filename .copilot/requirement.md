# 🚀 Application Requirements: Pacific Video & Audio Website

## 📌 Overview

- **App Name**: Pacific Video & Audio Website
- **Platform**: Blazor .NET 9.0, SQL Server 2022
- **Purpose**: Retail website, as well as information about our services available.

---

## ✅ Functional Requirements

### 🌐 Website [Website](http://www.pacificvideoaudio.com)

- The site will have both an informational and retail section of the website.
- Users will be able to select between different packages for Video and Audio equipment as well as sign up for online or offline assistance with installing and working with both video and audio equipment.
- Users will have access to information and video tutorials for using the types of equipment that we sell.
- Site should have a fulling functional shopping cart and the ability for the customer to checkout using either PayPal or Stripe.


### 🛒 Shopping Cart

- Allow user to select a given package or individual products.
- During checkout, customer has the ability to select an installation plan or self-install if they wish.
- During checkout, customer will be given the option of purchasing installation and equipment training time along with purchase at a discount.
- Provide a persistent cart icon showing the number of items in the cart at all times.
- Cart contents should be saved across sessions for logged-in users.
- Implement a "Guest Checkout" option (checkout without account creation).
- Provide functionality to apply promo codes or discounts at checkout.
- Support estimated shipping cost calculation based on location (if physical products are shipped).
- Display taxes, shipping, and final total before confirming purchase.
- Clearly display return and refund policy during checkout.

### 👤 User Accounts

- Customers can create an account to track orders, manage saved addresses, and view purchase history.
- Secure password handling using industry best practices.
- Password reset functionality via email.
- Admin accounts can manage orders, products, and promotional offers.

### 💳 Payment

- Integrate PayPal and Stripe for secure checkout.
- Payment processing must comply with PCI-DSS standards.
- Support for major credit/debit cards through Stripe.
- Clearly display confirmation screen and email receipt upon successful purchase.

### 📦 Order Management

- Order confirmation emails sent to customers after purchase.
- Admin portal for viewing and managing orders.
- Ability for admins to mark orders as shipped, completed, or refunded.


### 🧰 Technical Requirements

- Use MVVM Community Toolkit for binding and command handling.
- Use Dependency Injection for services. Making sure to keep within design standards for separating dependencies into each of there own projects.
- Use the onion layer principle when designing the application.
- Implement database access using EF Core 9.
- Navigation will be menu on the top center.

### 🧰 Additional Technical Requirements

- Ensure site is responsive and mobile-friendly.
- Follow accessibility standards (WCAG 2.1) where possible.
- SEO-friendly URLs and metadata for products and pages.
- Basic analytics integration (e.g., Google Analytics).
- Implement error handling and logging.






