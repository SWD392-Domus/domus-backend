namespace Domus.Common.Helpers;

public static class NotificationHelper
{
    public static string ADMIN_ID = "ba33e296-1ffa-402a-9495-fc70bb26e091";
    public static string CreateContractMessage(string contractorUserFullName, Guid newContractId, Guid requestQuotationRevisionId)
    {
        return $"Staff [{contractorUserFullName}] has just created a contract with ID: {newContractId.ToString().Substring(0,3)} correspond with the quotation revision's ID:{requestQuotationRevisionId}. Please check your contract again before signing.";
    }

    public static string CreateSignedContractMessage(string clientFullName, string contractClientId, Guid contractId)
    {
        return $"client [${clientFullName}] with Id:[{contractClientId.Substring(0,3)}] has just signed a contract with ID: {contractId.ToString().Substring(0,3)}. Please check the contract again and contact with the customer.";
    }

    public static string CreateDeletedContractMessage(Guid contractId, string contractorName)
    {
        return $"Your contract with Id [${contractId.ToString().Substring(0,3)}] has just been deleted by a staff [{contractorName}]. Please contact with us in case of problems.";
    }

    public static string CreateNegotiationMessageForCustomer(string staffName, Guid quotationId)
    {
        return $"The staff [{staffName}] has just replied your quotation request negotiation at Quotation [${quotationId.ToString().Substring(0,3)}]. Please check the quotation again and feel free to contact with us.";
    }

    public static string CreateNegotiationMessageForStaff(string customerName, Guid quotationId)
    {
        return $"The client [{customerName}] has just replied your quotation request negotiation at Quotation [${quotationId.ToString().Substring(0,3)}]. Please check the quotation again.";
    }

    public static string CreateUpdatedQuotationMessage(string quotationCustomerId, Guid quotationId)
    {
        return $"The client [{quotationCustomerId}] has just updated the Quotation [{quotationId.ToString().Substring(0,3)}]. Please check the quotation again.";
    }

    public static string CreateDeletedQuotation(Guid quotationId, string staffName)
    {
        return $"The Staff [{staffName}] has just deleted the Quotation [{quotationId.ToString().Substring(0,3)}]. Please check the quotation again.";
    }

    public static string CreateNewQuotationMessage(string customerName, Guid quotationId)
    {
        return $"The Client [{customerName}] has just requested an Quotation[${quotationId.ToString().Substring(0,3)}]. Please check the quotation and assign a staff for this quotation.";
    }
}