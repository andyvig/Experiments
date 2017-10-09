# EncodingCompare
Compares xml, json, and protobuf encoding payload sizes for various types of payloads.

## Notes 
* Uncompressed, xml is the largest, then json, then protobuf (for non-string-heavy payloads) with a large margin between each.
* Compressed (via gzip) they are all very similar
* protobuf doesn't optimize for strings

## Example Output
---Comparing MixedPayload[] Encoding---  
Count=300 Compressed=False  
Xml length 57109  
Json length 38072  
Protobuf length 15900  

---Comparing NumericPayload[] Encoding---  
Count=300 Compressed=False  
Xml length 1298153  
Json length 569486  
Protobuf length 405136  

---Comparing FacebookPayload[] Encoding---  
Count=300 Compressed=False  
Xml length 276108  
Json length 247898  
Protobuf length 215932  

---Comparing MixedPayload[] Encoding---  
Count=300 Compressed=True  
Xml length 2070  
Json length 1818  
Protobuf length 1356  

---Comparing NumericPayload[] Encoding---  
Count=300 Compressed=True  
Xml length 115633  
Json length 106677  
Protobuf length 77594  

---Comparing FacebookPayload[] Encoding---  
Count=300 Compressed=True  
Xml length 89854  
Json length 88991  
Protobuf length 89070  
