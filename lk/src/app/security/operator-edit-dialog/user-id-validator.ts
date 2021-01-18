import {AbstractControl, ValidatorFn} from "@angular/forms";

export function userIdValidator(): ValidatorFn{
    return (currentControl: AbstractControl): { [key: string]: any } => {
        if (currentControl.value){
            if (currentControl.value === '00000000-0000-0000-0000-000000000000')
                return { 'userId': 'empty' };
            else if (currentControl.value.match(/[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/i))
                return null;
            else
                return { 'userId': 'not match GUID' };
        }

        return { 'userId': 'not set value' };
    }
}
