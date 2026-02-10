////////////////////////////////////////////////////////////////////////////
/**
 *  @file   main.cpp
 *  @brief  This is a basic bank program that allows users to create and manage accounts,
 */
////////////////////////////////////////////////////////////////////////////

#include <iostream>

#include <Parser.h>
#include <Utilities.h>

int main(int argc, char *argv[])
{
    bank::ParserResult result;

    std::cout << "Welcome to the Bank application. \n";
    std::cout << "Type --help to view support functionality\n";

    do
    {
        result = bank::Utilities::parseUserInput(argc, argv);

        if (result.status == bank::ParseStatus::HelpRequested)
        {
            // help already printed
            return 0;
        }

        if (result.status == bank::ParseStatus::ExitRequested)
        {
            std::cout << "Goodbye\n";
            return 0;
        }

        if (result.status == bank::ParseStatus::InvalidArguments)
        {
            std::cerr << "Error: " << result.error << "\n";
            std::cerr << "Use --help to see valid arguments\n";
            return 1;
        }

    } while (result.status != bank::ParseStatus::Ok);
}
