cmake_minimum_required(VERSION 3.10)

set(TARGET_NAME bank_test)
project(${TARGET_NAME} VERSION 1.0 LANGUAGES CXX)


# C++ standard (target-based is possible, but this is fine globally)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

include(FetchContent)

# Fetch latest stable GoogleTest
FetchContent_Declare(
    googletest
    GIT_REPOSITORY https://github.com/google/googletest.git
    GIT_TAG        v1.17.0
)
FetchContent_MakeAvailable(googletest)

enable_testing()

# Collect test sources
file(GLOB UNIT_TEST_SOURCES
    "*.cpp"
)

add_executable(${TARGET_NAME} ${UNIT_TEST_SOURCES})

# Coverage flags applied ONLY to this target
if(APPLE)
    target_compile_options(${TARGET_NAME} PRIVATE
        -fprofile-instr-generate
        -fcoverage-mapping
    )

    target_link_options(${TARGET_NAME} PRIVATE
        -fprofile-instr-generate
        -fcoverage-mapping
    )
else()
    target_compile_options(${TARGET_NAME} PRIVATE
        -g
        -O0
        -coverage
        -fprofile-arcs
        -ftest-coverage
    )

    target_link_options(${TARGET_NAME} PRIVATE
        -coverage
        -lgcov
    )
endif()


# Include your project headers
target_include_directories(${TARGET_NAME} PRIVATE
    ${CMAKE_CURRENT_SOURCE_DIR}/../inc
)

# Link GoogleTest + pthread
target_link_libraries(${TARGET_NAME} PRIVATE
    gtest_main
    gmock_main
    pthread
)
# Link your main project library
add_test(NAME ${TARGET_NAME} COMMAND ${TARGET_NAME})

# Add this for better CTest integration:
set_tests_properties(${TARGET_NAME} PROPERTIES
    LABELS "unit"
)