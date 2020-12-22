export function Security(policy: string){
    return (constructor: any) => {
        Object.defineProperty(constructor, '__securityPolicy', {
            enumerable: false,
            configurable: false,
            writable: false,
            value: policy
        });
    };
}
