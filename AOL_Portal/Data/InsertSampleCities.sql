-- Insert sample cities into the City table
INSERT INTO City (CityName, CityCode, CityCounty, CityCountry, CreatedDate)
VALUES 
    ('New York', 'NYC', 'New York County', 'United States', GETUTCDATE()),
    ('Los Angeles', 'LAX', 'Los Angeles County', 'United States', GETUTCDATE()),
    ('London', 'LON', 'Greater London', 'United Kingdom', GETUTCDATE()),
    ('Istanbul', 'IST', 'Istanbul Province', 'Turkey', GETUTCDATE()),
    ('Tokyo', 'TYO', 'Tokyo Prefecture', 'Japan', GETUTCDATE()),
    ('Paris', 'PAR', 'Île-de-France', 'France', GETUTCDATE()),
    ('Berlin', 'BER', 'Berlin', 'Germany', GETUTCDATE()),
    ('Dubai', 'DXB', 'Dubai', 'United Arab Emirates', GETUTCDATE()),
    ('Singapore', 'SIN', 'Singapore', 'Singapore', GETUTCDATE()),
    ('Sydney', 'SYD', 'New South Wales', 'Australia', GETUTCDATE());

