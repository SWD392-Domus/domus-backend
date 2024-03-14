namespace Domus.Common.Helpers;

public static class NotificationHelper
{
    public static string ADMIN_ID = "ba33e296-1ffa-402a-9495-fc70bb26e091";
    public static string CreateContractMessage(string contractorUserFullName, Guid newContractId, Guid requestQuotationRevisionId)
    {
        return $"Staff [{contractorUserFullName}] has just created a contract with ID: {newContractId} correspond with the quotation revision's ID:{requestQuotationRevisionId}. Please check your contract again before signing.";
    }

    public static string CreateSignedContractMessage(string clientFullName, string contractClientId, Guid contractId)
    {
        return $"client [${clientFullName}] with Id:[{contractClientId}] has just signed a contract with ID: {contractId}. Please check the contract again and contact with the customer.";
    }

    public static string CreateDeletedContractMessage(Guid contractId, string contractContractorId)
    {
        return $"Your contract with Id [${contractId}] has just been deleted by a contractor with ID: {contractContractorId}. Please contact with us in case of problems.";
    }

    public static string CreateNewQuotationMessage()
    {
        throw new NotImplementedException();
    }

    public static string CreateNegotiationMessageForCustomer(string quotationStaffId, Guid quotationId)
    {
        return $"The staff [{quotationStaffId}] has just replied your quotation request negotiation at Quotation [${quotationId}]. Please check the quotation again and feel free to contact with us.";
    }

    public static string CreateNegotiationMessageForStaff(string quotationCustomerId, Guid quotationId)
    {
        return $"The client [{quotationCustomerId}] has just replied your quotation request negotiation at Quotation [${quotationId}]. Please check the quotation again.";
    }

    public static string CreateUpdatedQuotationMessage(string quotationCustomerId, Guid quotationId)
    {
        return $"The client [{quotationCustomerId}] has just updated the Quotation [{quotationId}]. Please check the quotation again.";
    }
}