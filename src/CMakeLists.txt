# Auto-discover library source files (excluding main.cpp)
file(GLOB LIBRARY_SOURCES "*.cpp")
list(REMOVE_ITEM LIBRARY_SOURCES "${CMAKE_CURRENT_SOURCE_DIR}/main.cpp")

# Library outputs will inherit the centralized directories from the top-level
# CMake file; avoid overriding them here to keep a single output location.

add_library(banklib STATIC ${LIBRARY_SOURCES})
target_include_directories(banklib PUBLIC ${PROJECT_SOURCE_DIR}/inc)

add_executable(${PROJECT_NAME} main.cpp)
target_link_libraries(${PROJECT_NAME} PRIVATE banklib)