cmake_minimum_required(VERSION 3.10)
project(bank_test VERSION 1.0 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

include(FetchContent)

FetchContent_Declare(googletest
    GIT_REPOSITORY https://github.com/google/googletest.git
    GIT_TAG v1.17.0
)
FetchContent_MakeAvailable(googletest)

enable_testing()
# Automatically collect all test source files
## Auto-discover test source files across the project. Use a recursive glob to
## pick up any newly created test source that follows a test naming pattern.
file(GLOB_RECURSE UNIT_TEST_SOURCES CONFIGURE_DEPENDS
    "${PROJECT_SOURCE_DIR}/*Test.cpp"
)

# Ensure the test runner main is included (it's not a *Test.cpp file).
set(TEST_MAIN_SRC "${CMAKE_CURRENT_SOURCE_DIR}/mainGtest.cpp")
list(APPEND UNIT_TEST_SOURCES ${TEST_MAIN_SRC})

add_executable(${PROJECT_NAME} ${UNIT_TEST_SOURCES})

# Place the test executable into the centralized runtime output directory so
# all built executables live in a single, consistent location.
set_target_properties(${PROJECT_NAME} PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}
)

if(APPLE)
    target_compile_options(${PROJECT_NAME} PRIVATE -fprofile-instr-generate -fcoverage-mapping)
    target_link_options(${PROJECT_NAME} PRIVATE -fprofile-instr-generate -fcoverage-mapping)
else()
    target_compile_options(${PROJECT_NAME} PRIVATE -g -O0 -coverage -fprofile-arcs -ftest-coverage)
    target_link_options(${PROJECT_NAME} PRIVATE -coverage -lgcov)
endif()

target_include_directories(${PROJECT_NAME} PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/../inc ${CMAKE_CURRENT_SOURCE_DIR}/Inc)

target_link_libraries(${PROJECT_NAME} PRIVATE gtest_main gmock_main pthread banklib)

add_test(NAME ${PROJECT_NAME} COMMAND ${PROJECT_NAME})
set_tests_properties(${PROJECT_NAME} PROPERTIES LABELS "unit")