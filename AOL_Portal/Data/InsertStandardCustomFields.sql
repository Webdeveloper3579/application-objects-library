-- Insert standard custom fields for customers
-- These fields will be automatically created for each new user during sign-up

INSERT INTO AolCustomerCustomFields (
    CustomerCustomFieldName,
    CustomerCustomType,
    CustomerCustomFieldLabel,
    CustomerCustomFieldDescription,
    CustomerCustomFieldType,
    CustomerCustomFieldStatus,
    CreatedDate,
    ModifiedDate
) VALUES 
(
    'CustomerName',
    'Standard',
    'Customer Name',
    'for capture customer name',
    'text',
    'released',
    GETUTCDATE(),
    NULL
),
(
    'CustomerType',
    'Standard',
    'Customer Type',
    'for capture customer Type',
    'text',
    'released',
    GETUTCDATE(),
    NULL
),
(
    'Address',
    'Standard',
    'Address',
    'for capture Address',
    'textarea',
    'released',
    GETUTCDATE(),
    NULL
);
