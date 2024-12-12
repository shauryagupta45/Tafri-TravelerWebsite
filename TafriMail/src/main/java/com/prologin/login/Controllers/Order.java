package com.prologin.login.Controllers;

import com.prologin.login.Services.GenerateOrder;
import com.razorpay.RazorpayException;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import java.util.HashMap;
import java.util.Map;

@RestController
public class Order {
    @PostMapping("/generateOrder")
    public Map<String , String> generateOrder(@RequestBody Map<String, String> data){
        System.out.println(data);
        String amount = data.get("Amount");
//        String userId = data.get("UserId");
        String bookingId = data.get("BookingId");
        String generatedOrderId = null;

        Map<String, String> response = new HashMap<>();

        try {
            generatedOrderId = GenerateOrder.placeOrderToMerchant(Integer.valueOf(amount) , Integer.valueOf(bookingId));
            response.put("message", "Order Generated Successfully");
            response.put("status", "true");
            response.put("orderId" , generatedOrderId);
        } catch (RazorpayException e) {
            response.put("message", "Error Generating Order");
            response.put("status", "false");
            response.put("error" , e.getMessage());
        }

        return response;
    }

}
