# Collect source files (excluding main.cpp)
set(LIBRARY_SOURCES
    Account.cpp
)

# Set the output folder where your program will be created
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/lib)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/lib)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)

# Create static library with unique name to avoid CMake conflicts
add_library(banklib STATIC ${LIBRARY_SOURCES})

target_include_directories(banklib PUBLIC
    ${PROJECT_SOURCE_DIR}/inc
)

# Create executable
add_executable(${PROJECT_NAME} main.cpp)

target_link_libraries(${PROJECT_NAME} PRIVATE banklib)