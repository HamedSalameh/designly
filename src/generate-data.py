import requests
import jwt  # PyJWT library to decode JWT tokens

# Authentication details
auth_url = "https://localhost:7119/api/v1/Identity/signin"
auth_payload = {
    "username": "hamedsalami@gmail.com",
    "password": "xV&jH58L9wb@Ze"
}

# Authentication headers for form-encoded data
auth_headers = {
    "Content-Type": "application/x-www-form-urlencoded"
}

# Sign in and get the authentication token (using form-encoded data)
auth_response = requests.post(auth_url, data=auth_payload, headers=auth_headers, verify=False)

if auth_response.status_code == 200:
    token_data = auth_response.json()
    access_token = token_data.get("access_token")
    print("Authenticated successfully.")
else:
    print(f"Failed to authenticate: {auth_response.status_code}, {auth_response.text}")
    exit()

# Decode the JWT token to extract tenant information
decoded_token = jwt.decode(access_token, options={"verify_signature": False})  # Disable signature verification for decoding
tenant_id = decoded_token.get("TenantId")

if not tenant_id:
    print("Failed to extract TenantId from token.")
    exit()

# Set headers for authenticated requests
headers = {
    "Authorization": f"Bearer {access_token}",
    "Content-Type": "application/json"
}

# Client creation URL
client_creation_url = "https://localhost:7119/api/v1/clients"

# List of client data (TenantId is now extracted from the token)
clients = [
    {
        "TenantId": tenant_id,  # Using the extracted TenantId
        "FirstName": "איתן",
        "FamilyName": "לוי",
        "Address": {
            "City": "חיפה",
            "Street": "מוריה",
            "BuildingNumber": "23",
            "AddressLines": [
                "בניין גפן"
            ]
        },
        "ContactDetails": {
            "PrimaryPhoneNumber": "050-1234567",
            "SecondaryPhoneNumber": "052-7654321",
            "EmailAddress": "eitan.levi@example.com"
        }
    },
    {
        "TenantId": tenant_id,
        "FirstName": "נועה",
        "FamilyName": "כהן",
        "Address": {
            "City": "קריית שמונה",
            "Street": "ההגנה",
            "BuildingNumber": "12",
            "AddressLines": [
                "בניין דקל"
            ]
        },
        "ContactDetails": {
            "PrimaryPhoneNumber": "050-2345678",
            "SecondaryPhoneNumber": "052-8765432",
            "EmailAddress": "noa.cohen@example.com"
        }
    },
    {
        "TenantId": tenant_id,
        "FirstName": "יונתן",
        "FamilyName": "ברק",
        "Address": {
            "City": "עכו",
            "Street": "הרצל",
            "BuildingNumber": "45",
            "AddressLines": [
                "בניין ברוש"
            ]
        },
        "ContactDetails": {
            "PrimaryPhoneNumber": "050-3456789",
            "SecondaryPhoneNumber": "052-9876543",
            "EmailAddress": "yonatan.barak@example.com"
        }
    },
    {
        "TenantId": tenant_id,
        "FirstName": "מאיה",
        "FamilyName": "אבגני",
        "Address": {
            "City": "נהריה",
            "Street": "שדרות הגעתון",
            "BuildingNumber": "67",
            "AddressLines": [
                "בניין ארז"
            ]
        },
        "ContactDetails": {
            "PrimaryPhoneNumber": "050-4567890",
            "SecondaryPhoneNumber": "052-0987654",
            "EmailAddress": "maya.avgeni@example.com"
        }
    },
    {
        "TenantId": tenant_id,
        "FirstName": "אורי",
        "FamilyName": "מלכה",
        "Address": {
            "City": "טבריה",
            "Street": "הגליל",
            "BuildingNumber": "89",
            "AddressLines": [
                "בניין שקד"
            ]
        },
        "ContactDetails": {
            "PrimaryPhoneNumber": "050-5678901",
            "SecondaryPhoneNumber": "052-1098765",
            "EmailAddress": "uri.malka@example.com"
        }
    }
]

# Create clients
for client in clients:
    response = requests.post(client_creation_url, json=client, headers=headers, verify=False)
    
    if response.status_code == 201:
        print(f"Client {client['FirstName']} {client['FamilyName']} created successfully.")
    else:
        print(f"Failed to create client {client['FirstName']} {client['FamilyName']}: {response.status_code}, {response.text}")
