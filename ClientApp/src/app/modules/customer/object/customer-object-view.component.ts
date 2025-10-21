import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { UserService } from 'app/core/user/user.service';

export interface Field {
    Id: number;
    ObjectId: number;
    FieldName: string;
    FieldLabel: string;
    FieldType: string;
    FieldDescription: string;
    FieldStatus: string;
    FieldEnum: string;
    CreatedDate?: string;
    ModifiedDate?: string;
}

export interface Object {
    Id: number;
    ObjectName: string;
    ObjectEnum: string;
    CreatedDate?: string;
    ModifiedDate?: string;
}

export interface FieldValue {
    Id: number;
    UserId: string;
    ObjectId: number;
    FieldId: number;
    Value: string;
    CreatedDate?: string;
    ModifiedDate?: string;
}

export interface SelectValue {
    Id: number;
    FieldId: number;
    Value: string;
    CreatedDate?: string;
    ModifiedDate?: string;
}

@Component({
    selector     : 'customer-object-view',
    standalone   : true,
    imports      : [CommonModule, FormsModule],
    templateUrl  : './customer-object-view.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class CustomerObjectViewComponent implements OnInit
{
    objectId: number | null = null;
    object: Object | null = null;
    fields: Field[] = [];
    fieldValues: FieldValue[] = [];
    selectValues: SelectValue[] = [];
    loading = true;
    errorMessage: string | null = null;
    successMessage: string | null = null;
    saving = false;

    // Form data - will be populated with field values
    formData: { [key: number]: string } = {};
    
    // Cache select values by field ID for better performance
    fieldSelectValues: { [fieldId: number]: SelectValue[] } = {};

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private http: HttpClient,
        private userService: UserService
    ) {}

    ngOnInit(): void
    {
        // Get object ID from route parameters
        this.route.params.subscribe(params => {
            this.objectId = +params['id'];
            if (this.objectId) {
                this.loadObjectData();
            }
        });
    }

    loadObjectData(): void
    {
        if (!this.objectId) return;

        this.loading = true;
        this.errorMessage = null;

        // Load object details, fields, select values, and existing field values
        Promise.all([
            this.http.get<Object>(`/api/Object/${this.objectId}`).toPromise(),
            this.http.get<Field[]>(`/api/Field/object/${this.objectId}`).toPromise(),
            this.http.get<SelectValue[]>('/api/SelectValue').toPromise(),
            this.http.get<FieldValue[]>(`/api/FieldValue/object/${this.objectId}`).toPromise()
        ]).then(([object, fields, selectValues, fieldValues]) => {
            this.object = object || null;
            this.fields = fields || [];
            this.selectValues = selectValues || [];
            this.fieldValues = fieldValues || [];
            
            // Cache select values by field ID
            this.fieldSelectValues = {};
            this.fields.forEach(field => {
                this.fieldSelectValues[field.Id] = this.selectValues.filter(sv => sv.FieldId === field.Id);
            });
            
            // Initialize form data with existing values
            this.initializeFormData();
            
            this.loading = false;
        }).catch(error => {
            console.error('Error loading object data:', error);
            this.errorMessage = 'Failed to load object data';
            this.loading = false;
        });
    }

    initializeFormData(): void
    {
        this.formData = {};
        
        // Initialize with existing field values
        this.fieldValues.forEach(fieldValue => {
            this.formData[fieldValue.FieldId] = fieldValue.Value;
        });
        
        // Initialize empty values for fields without existing data
        this.fields.forEach(field => {
            if (!(field.Id in this.formData)) {
                this.formData[field.Id] = '';
            }
        });
    }

    getSelectValuesForField(fieldId: number): SelectValue[]
    {
        const values = this.fieldSelectValues[fieldId] || [];
        return values;
    }

    getFieldValueId(fieldId: number): number | null
    {
        const fieldValue = this.fieldValues.find(fv => fv.FieldId === fieldId);
        return fieldValue ? fieldValue.Id : null;
    }

    isSelectField(field: Field): boolean
    {
        return field.FieldType.toLowerCase() === 'select';
    }

    onFieldValueChange(fieldId: number, value: string): void
    {
        this.formData[fieldId] = value;
    }

    saveFieldValues(): void
    {
        if (!this.objectId) return;

        this.saving = true;
        this.errorMessage = null;
        this.successMessage = null;

        // Get current user ID from user service
        const currentUser = this.userService.user;
        const userId = currentUser?.id || 'unknown-user';

        // Prepare field values to save
        const fieldValuesToSave: FieldValue[] = [];
        
        Object.keys(this.formData).forEach(fieldIdStr => {
            const fieldId = +fieldIdStr;
            const value = this.formData[fieldId] || '';
            
            // Check if this field value already exists
            const existingValue = this.fieldValues.find(fv => fv.FieldId === fieldId);
            
            if (existingValue) {
                // Update existing value (even if empty to clear it)
                existingValue.Value = value.trim();
                existingValue.ModifiedDate = new Date().toISOString();
                fieldValuesToSave.push(existingValue);
            } else if (value.trim()) {
                // Only create new value if it's not empty
                fieldValuesToSave.push({
                    Id: 0, // Will be set by backend
                    UserId: userId,
                    ObjectId: this.objectId!,
                    FieldId: fieldId,
                    Value: value.trim(),
                    CreatedDate: new Date().toISOString()
                });
            }
        });

        if (fieldValuesToSave.length === 0) {
            this.saving = false;
            this.errorMessage = 'No changes to save';
            return;
        }

        // Save field values
        this.http.post('/api/FieldValue/multiple', fieldValuesToSave).subscribe({
            next: (response) => {
                console.log('Field values saved successfully:', response);
                this.saving = false;
                this.successMessage = 'Field values saved successfully!';
                
                // Reload data to get updated values
                this.loadObjectData();
                
                // Clear success message after 3 seconds
                setTimeout(() => {
                    this.successMessage = null;
                }, 3000);
            },
            error: (error) => {
                console.error('Error saving field values:', error);
                this.saving = false;
                this.errorMessage = 'Failed to save field values: ' + (error.message || 'Unknown error');
            }
        });
    }

    goBack(): void
    {
        this.router.navigate(['/customer']);
    }
}
