using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Locations.GetCountries;
using RetirementTime.Application.Features.Subdivisions.GetSubdivisions;
using RetirementTime.Application.Features.Users.CreateUser;
using RetirementTime.Domain.Entities.Location;
using RetirementTime.Models.Auth;
using RetirementTime.Resources.Auth;

namespace RetirementTime.Components.Pages.Auth;

public partial class SignUp
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IStringLocalizer<AuthResources> Localizer { get; set; } = default!;

    private List<Country> _countries = new();
    private List<Subdivision> _subdivisions = new();
    private readonly SignUpModel _signupModel = new();
    private string _errorMessage = string.Empty;
    private string _emailErrorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        _countries = await Mediator.Send(new GetCountriesQuery());
        if (_countries.Any())
        {
            _subdivisions = await Mediator.Send(new GetSubdivisionsQuery(){CountryId = _countries.First().Id});
            _signupModel.Country = _countries.First().Id;
            
            if(_subdivisions.Any())
                _signupModel.State = _subdivisions.First().Id;
        }
    }

    private async Task HandleSignUp()
    {
        // Clear any previous error messages
        _errorMessage = string.Empty;
        _emailErrorMessage = string.Empty;
        
        var command = new CreateUserCommand
        {
            Email = _signupModel.Email,
            Password = _signupModel.Password,
            FirstName = _signupModel.FirstName,
            LastName = _signupModel.LastName,
            CountryId = _signupModel.Country,
            SubdivisionId = _signupModel.State
        };

        var result = await Mediator.Send(command);

        if (result.Success)
        {
            Navigation.NavigateTo("/login");
        }
        else
        {
            // Check if error is email-related
            if (result.ErrorMessage?.Contains("email", StringComparison.OrdinalIgnoreCase) == true)
            {
                _emailErrorMessage = result.ErrorMessage;
            }
            else
            {
                _errorMessage = result.ErrorMessage ?? "An error occurred during signup. Please try again.";
            }
        }
    }

    private async Task HandleCountryChanged()
    {
        if (_signupModel.Country != 0 && _countries.Any() && _countries.Any(c => c.Id == _signupModel.Country))
        {
            _subdivisions = await Mediator.Send(new GetSubdivisionsQuery(){CountryId = _signupModel.Country});
        }
        else
        {
            _subdivisions.Clear();
        }
    }
}

