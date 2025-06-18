# Car Rentals - Xamarin.Forms Application

A comprehensive cross-platform car rental mobile application built with Xamarin.Forms, featuring user authentication, car browsing, filtering, and rental booking functionality.

## 🚗 Features

### Core Functionality
- **User Authentication**: Secure login system with demo accounts
- **Car Browsing**: Browse available vehicles with detailed information
- **Search & Filter**: Search cars by brand/model and filter by category
- **Car Details**: View comprehensive car specifications and pricing
- **Rental Booking**: Book cars with date selection and cost calculation
- **Responsive UI**: Modern, user-friendly interface with Material Design

### Car Categories
- Economy
- Compact
- Mid-size
- Full-size
- SUV
- Luxury

### Car Information Displayed
- Brand and Model
- Year, Color, Transmission
- Fuel Type, Mileage, Seats
- Daily Rate and Availability
- Category and Description

## 🛠 Technology Stack

- **Framework**: Xamarin.Forms 5.0.0.2196
- **Platform**: Cross-platform (Android, iOS)
- **Architecture**: MVVM (Model-View-ViewModel) Pattern
- **Target Framework**: .NET Standard 2.0
- **UI**: XAML with Shell Navigation

## 📱 Screenshots

### Login Screen
- Clean, modern login interface
- Demo credentials provided
- Error handling and validation

### Cars List
- Card-based car display
- Search and filter functionality
- Real-time availability status

### Car Details
- Comprehensive car specifications
- Rental date selection
- Cost calculation
- Booking functionality

## 🔧 Project Structure

```
Car_Rentals/
├── Models/
│   ├── Car.cs              # Car entity model
│   ├── Customer.cs         # Customer information model
│   ├── Rental.cs           # Rental transaction model
│   └── User.cs             # User authentication model
├── Services/
│   ├── ICarDataStore.cs    # Car data operations interface
│   ├── IAuthService.cs     # Authentication service interface
│   ├── MockCarDataStore.cs # Mock car data implementation
│   └── MockAuthService.cs  # Mock authentication implementation
├── ViewModels/
│   ├── BaseViewModel.cs    # Base MVVM functionality
│   ├── LoginViewModel.cs   # Login logic
│   ├── CarsViewModel.cs    # Car listing logic
│   └── CarDetailViewModel.cs # Car details and booking logic
├── Views/
│   ├── LoginPage.xaml      # Login screen
│   ├── CarsPage.xaml       # Car listing screen
│   ├── CarDetailPage.xaml  # Car details screen
│   ├── MyRentalsPage.xaml  # Rental history screen
│   └── AboutPage.xaml      # About information screen
└── Converters/
    └── BoolToColorConverter.cs # UI value converter
```

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019/2022 with Xamarin workload
- Android SDK (for Android development)
- Xcode (for iOS development, macOS only)

### Installation
1. Clone the repository
2. Open `Car_Rentals.sln` in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. Run on Android emulator or device

### Demo Credentials
- **Regular User**: `johndoe` / `password123`
- **Admin User**: `admin` / `admin123`

## 📊 Sample Data

The application includes mock data with 6 sample cars:
- Toyota Camry (Mid-size)
- Honda Civic (Compact)
- Ford Explorer (SUV)
- BMW 3 Series (Luxury)
- Nissan Altima (Mid-size) - Not available
- Chevrolet Spark (Economy)

## 🔐 Authentication

The app implements a mock authentication system with:
- User registration and login
- Session management
- Logout functionality
- Customer profile management

## 🎨 UI/UX Features

- **Material Design**: Modern, clean interface
- **Responsive Layout**: Adapts to different screen sizes
- **Color-coded Status**: Visual availability indicators
- **Card-based Design**: Easy-to-scan car information
- **Intuitive Navigation**: Shell-based navigation system

## 🔄 State Management

- **MVVM Pattern**: Clean separation of concerns
- **Observable Collections**: Real-time UI updates
- **Command Pattern**: User interaction handling
- **Dependency Injection**: Service registration and management

## 📈 Future Enhancements

### Planned Features
- [ ] Real backend API integration
- [ ] Payment processing
- [ ] Push notifications
- [ ] Offline support
- [ ] Image carousel
- [ ] User reviews and ratings
- [ ] Advanced filtering options
- [ ] Rental history management
- [ ] Admin panel for car management

### Technical Improvements
- [ ] Database integration (SQLite)
- [ ] Real authentication service
- [ ] Image caching and optimization
- [ ] Performance optimization
- [ ] Unit testing
- [ ] CI/CD pipeline

## 🐛 Known Issues

- Currently uses mock data (no real backend)
- Image placeholders instead of real car images
- Limited error handling in some areas
- No offline functionality

## 📄 License

This project is for demonstration purposes. Feel free to use as a reference for your own car rental applications.

## 🤝 Contributing

This is a demonstration project, but suggestions and improvements are welcome!

## 📞 Support

For questions or support, please refer to the Xamarin.Forms documentation or create an issue in the repository.

---

**Built with ❤️ using Xamarin.Forms** 