namespace RetirementTime.Models.Introduction;

public class OnboardingDataViewModel
{
    // Step 1: Personal Info
    public PersonalInfoModel? PersonalInfo { get; set; }

    // Step 2: Assets
    public AssetsModel? Assets { get; set; }

    // Step 3: Debt
    public DebtModel? Debt { get; set; }

    // Step 4: Employment
    public EmploymentModel? Employment { get; set; }

    // Helper methods to check completion status
    public bool IsStep0Complete => PersonalInfo != null; // Complete if user has moved past welcome

    public bool IsStep1Complete => PersonalInfo != null 
                                    && PersonalInfo.DateOfBirth.HasValue 
                                    && !string.IsNullOrWhiteSpace(PersonalInfo.CitizenshipStatus) 
                                    && !string.IsNullOrWhiteSpace(PersonalInfo.MaritalStatus);

    public bool IsStep2Complete => Assets != null;

    public bool IsStep3Complete => Debt != null;

    public bool IsStep4Complete => Employment != null 
                                    && Employment.PlannedRetirementAge.HasValue 
                                    && Employment.PlannedRetirementAge.Value > 0;
}
