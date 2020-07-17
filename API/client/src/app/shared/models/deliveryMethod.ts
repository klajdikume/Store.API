import { NgIf } from '@angular/common';

export interface IDeliveryMethod {
    shortName: string;
    deliveryTime: string;
    description: string;
    price: number;
    id: number;
}
