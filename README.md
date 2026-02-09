## Purpose

This is a C++ banking application project with comprehensive unit testing using Google Test (gtest) and code coverage analysis. The project demonstrates modern C++ development practices with CMake build automation, mocking capabilities via Google Mock, and test coverage tracking.

## Project Structure

```
.
├── inc/                 # Header files
│   └── accountType.h
├── src/                 # Source files
│   ├── main.cpp
│   └── CMakeLists.txt
├── test/                # Unit tests
│   ├── mainGtest.cpp
│   └── CMakeLists.txt
├── CmakeLists.txt       # Root CMake configuration
├── CMakeCache.txt       # CMake cache (generated)
└── CMakeFiles/          # CMake build artifacts (generated)
```

## Prerequisites

- CMake 3.9 or higher
- C++17 compatible compiler (clang, gcc, or MSVC)
- Git (for fetching GoogleTest)
- `gcov` and `lcov` (optional, for coverage reports)

### macOS Installation

```bash
brew install cmake
brew install lcov  # For coverage reports
```

### Linux Installation (Ubuntu/Debian)

```bash
sudo apt-get install cmake build-essential
sudo apt-get install lcov  # For coverage reports
```

## Building the Project

### Create a build directory

```bash
mkdir -p build
cd build
```

### Configure with CMake

```bash
cmake ..
```

### Build the project

```bash
cmake --build .
```

Or using make directly:

```bash
make
```

## Running Unit Tests

### Run all tests with verbose output

```bash
ctest --verbose
```

### Run tests directly

```bash
./test/bank_test
```

### After adding new TEST_F cases to any test file, always do a clean rebuild
rm -rf build
cmake -B build -S .
cmake --build build --target bank_test


or 
### Run specific test Or use the VSCode task:

```bash
./build/bin/bank_test --gtest_filter=YourTestFixtureName*
```

build-tests

## Code Coverage

### Generate coverage data

After building and running tests:

```bash
cd build
./test/bank_test
```

### Generate coverage report

```bash
# Generate coverage files
gcov ../test/*.cpp

# Create LCOV info file
lcov --capture --directory . --output-file coverage.info

# Generate HTML report
genhtml coverage.info --output-directory coverage_report
```

View the coverage report:

```bash
open coverage_report/index.html  # macOS
# or
firefox coverage_report/index.html  # Linux
```

## Clean Build

To perform a clean rebuild:

```bash
rm -rf build
mkdir build
cd build
cmake ..
cmake --build .
ctest --verbose
```

## CMake Configuration Details

- **C++ Standard**: C++17
- **GoogleTest Version**: release-1.11.0
- **Coverage Flags**: `-coverage -fprofile-arcs -ftest-coverage`
- **Optimization Level**: -O0 (for coverage accuracy)
- **Debug Symbols**: -g (enabled)

## Project Configuration

The project uses:
- **GoogleTest** for unit testing and mocking
- **CMake FetchContent** for dependency management
- **Code Coverage** via gcov and lcov
- **GoogleMock** for creating mock objects

## Contributing

1. Write unit tests for all new features
2. Ensure all tests pass: `ctest --verbose`
3. Maintain or improve code coverage
4. Follow C++17 standards

## License

[Add your license information here]