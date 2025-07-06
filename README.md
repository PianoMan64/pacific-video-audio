# Pacific Video & Audio Website

A modern, responsive e-commerce website built with Blazor .NET 9.0 for Pacific Video & Audio - your trusted partner for professional video and audio solutions.

## 🚀 Features

### Current Implementation
- **Modern Design**: Clean, professional layout with video background hero section
- **Responsive Layout**: Mobile-first design that works on all devices
- **Product Showcase**: Dedicated sections for video and audio equipment
- **Service Packages**: Three-tier pricing structure (Starter, Professional, Enterprise)
- **Video Integration**: Utilizes existing video assets for marketing impact
- **Navigation**: Sticky header with intuitive menu structure

### Planned Features (Roadmap)
- **E-commerce Functionality**: Full shopping cart and checkout system
- **Payment Integration**: PayPal and Stripe payment processing
- **User Accounts**: Customer registration, login, and order history
- **Product Management**: Admin portal for inventory and order management
- **Service Booking**: Online scheduling for installation and training
- **Tutorial Platform**: Video learning management system

## 🛠️ Technology Stack

- **Frontend**: Blazor Server (.NET 9.0)
- **Styling**: Custom CSS with modern responsive design
- **Database**: SQL Server 2022 (planned)
- **Payment**: PayPal & Stripe integration (planned)
- **Architecture**: Onion Layer principle with MVVM Community Toolkit

## 📁 Project Structure

```
PVA/
├── PVA/                          # Main Blazor project
│   ├── Components/
│   │   ├── Layout/
│   │   │   └── MainLayout.razor  # Site layout with header/footer
│   │   └── Pages/
│   │       ├── Home.razor        # Homepage with video hero
│   │       ├── Products.razor    # Product categories
│   │       ├── Packages.razor    # Service packages
│   │       └── Services.razor    # Service offerings
│   ├── wwwroot/
│   │   ├── video/
│   │   │   └── Produce.mp4       # Hero background video
│   │   ├── images/               # Image assets
│   │   ├── app.css              # Base styles
│   │   └── styles.css           # Custom theme styles
│   └── Program.cs               # Application entry point
├── PVA.Client/                  # Blazor WebAssembly project
└── PVA.sln                     # Solution file
```

## 🎨 Design System

### Color Palette
- **Primary Red**: `#e74c3c` - Brand color for CTAs and highlights
- **Dark Blue**: `#2c3e50` - Headers and navigation
- **Light Gray**: `#f8f9fa` - Section backgrounds
- **Text Gray**: `#7f8c8d` - Body text

### Typography
- **Font Stack**: Segoe UI, Tahoma, Geneva, Verdana, sans-serif
- **Responsive Sizing**: Fluid typography scales with viewport

## 🚦 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server 2022 (for future database features)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/pacific-video-audio.git
   cd pacific-video-audio
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run --project PVA
   ```

4. **Open in browser**
   Navigate to `https://localhost:5001` or `http://localhost:5000`

## 📋 Development Roadmap

### Phase 1: Foundation (✅ Complete)
- [x] Project setup and architecture
- [x] Responsive design system
- [x] Homepage with video integration
- [x] Product and service pages
- [x] Navigation structure

### Phase 2: E-commerce Core (Planned)
- [ ] Shopping cart functionality
- [ ] Product catalog with database
- [ ] User authentication system
- [ ] Payment gateway integration
- [ ] Order management system

### Phase 3: Advanced Features (Planned)
- [ ] Admin dashboard
- [ ] Service booking system
- [ ] Tutorial platform
- [ ] Inventory management
- [ ] Customer support portal

### Phase 4: Optimization (Planned)
- [ ] Performance optimization
- [ ] SEO enhancements
- [ ] Analytics integration
- [ ] Progressive Web App features

## 🧰 Technical Requirements

As specified in requirements.md:
- **MVVM Community Toolkit** for binding and commands
- **Dependency Injection** with service separation
- **Onion Layer Architecture** for clean separation of concerns
- **Entity Framework Core 9** for data access
- **Responsive Design** with accessibility standards (WCAG 2.1)
- **SEO-friendly** URLs and metadata

## 📞 Support & Services

Pacific Video & Audio offers:
- **Professional Installation**: Certified technician setup
- **Equipment Training**: Comprehensive learning programs
- **Technical Support**: 24/7 assistance and maintenance
- **Consultation**: Expert advice for equipment selection

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is proprietary software for Pacific Video & Audio. All rights reserved.

## 📧 Contact

Pacific Video & Audio  
Website: [www.pacificvideoaudio.com](http://www.pacificvideoaudio.com)  
Email: info@pacificvideoaudio.com

---

**Note**: This is the initial design implementation. E-commerce functionality, payment processing, and user management features are planned for future releases.
