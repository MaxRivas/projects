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

file(GLOB UNIT_TEST_SOURCES "src/*.cpp" "Inc/*.cpp")
add_executable(${PROJECT_NAME} ${UNIT_TEST_SOURCES})

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