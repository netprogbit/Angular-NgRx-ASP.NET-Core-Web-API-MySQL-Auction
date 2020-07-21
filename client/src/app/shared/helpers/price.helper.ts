export class PriceHelper {

    // Needed in order to hint when editing and correctly backend converting
    public static format(price: number): string {

        let result = price.toString().replace(".", ",");

        if (result.indexOf(',') === -1)
            result = result + ',00';
        
        return result;
    }
}